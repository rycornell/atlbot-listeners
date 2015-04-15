namespace AtlBot.Listeners
{
    public interface IMessageListener
    {
        string MessagePattern { get; }

        string HelpText { get; }
        
        void HandleMessage(IMessage message);
    }
}
