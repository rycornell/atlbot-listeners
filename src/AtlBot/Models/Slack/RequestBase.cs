using System.Configuration;

namespace AtlBot.Models.Slack
{
    public abstract class RequestBase
    {
        protected RequestBase()
        {
            Token = ConfigurationManager.AppSettings["SlackToken"];
        }

        public string Token { get; set; }
    }
}