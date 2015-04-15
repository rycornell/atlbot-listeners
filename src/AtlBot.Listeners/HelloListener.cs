namespace AtlBot.Listeners
{
    public class HelloListener : IMessageListener
    {
        public string MessagePattern
        {
            get { return "hello"; }
        }

        public string HelpText
        {
            get { return "hello - say hello to the AtlBot"; }
        }

        public void HandleMessage(IMessage message)
        {
            message.Respond(new Response
            {
                Text = "Hello There!"
            });
        }
    }
}