using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AtlBot.Listeners
{
    public class HelpListener : IMessageListener
    {
        public string MessagePattern
        {
            get { return "help"; }
        }

        public string HelpText
        {
            get { return "help - displays help for the AtlBot"; }
        }

        public void HandleMessage(IMessage message)
        {
            var sb = new StringBuilder();

            var listeners = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(p => p.IsClass && typeof(IMessageListener).IsAssignableFrom(p));

            foreach (var messageListener in listeners)
            {
                var helpText = ((IMessageListener) Activator.CreateInstance(messageListener)).HelpText;
                sb.AppendLine(helpText);
            }

            message.Respond(new Response
            {
                Text = sb.ToString()
            });
        }
    }
}