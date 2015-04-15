using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using AtlBot.Listeners;
using AtlBot.Models;
using AtlBot.Models.Slack;
using AtlBot.Models.Slack.Chat;

namespace AtlBot.Controllers
{
    public class AtlBotController : ApiController
    {
        private readonly string _botName = ConfigurationManager.AppSettings["BotName"];
        private readonly IPostMessageService _postMessageService;

        public AtlBotController()
        {
            // TODO : setup some DI
            _postMessageService = new PostMessageService(new SlackClient());
        }

        // POST api/AtlBot
        public void Post([FromBody] AtlBotMessage message)
        {
            if (message == null || String.IsNullOrWhiteSpace(message.Text)) return;

            foreach (var messageListener in ListenerRegistration.Listeners.Where(x => !String.IsNullOrWhiteSpace(x.MessagePattern)))
            {
                if (!Regex.IsMatch(message.Text, messageListener.MessagePattern, RegexOptions.IgnoreCase)) continue;

                InvokeListener(messageListener, message);
            }
        }

        private void InvokeListener(IMessageListener messageListener, AtlBotMessage botMessage)
        {
            var listenerMessage = new Message
            {
                Text = botMessage.Text
            };

            // post to slack when the listener responds
            listenerMessage.ResponseRequested += (sender, args) =>
            {
                var post = new PostMessageRequest
                {
                    Channel = botMessage.ChannelId,
                    Username = _botName,
                    Text = args.Response.Text,
                    UnfurlLinks = true
                };

                var response = _postMessageService.PostMessage(post);
                if (!response.Ok)
                {
                    throw new ApplicationException("Error occurred when attempting to post message to Slack : " + response.Error);
                }
            };

            // run the listener in a new task
            Task.Factory.StartNew(() => messageListener.HandleMessage(listenerMessage), 
                TaskCreationOptions.LongRunning | TaskCreationOptions.PreferFairness)
                .ContinueWith(t =>
                {
                    if (t.Exception == null) return;
                    foreach (var e in t.Exception.InnerExceptions)
                    {
                        // log it
                        System.Diagnostics.Trace.WriteLine(e.Message);
                    }
                }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}