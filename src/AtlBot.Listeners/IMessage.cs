using System;

namespace AtlBot.Listeners
{
    public interface IMessage
    {
        event EventHandler<ResponseRequestedEventArgs> ResponseRequested;

        string Text { get; set; }

        void Respond(IResponse response);
    }
}
