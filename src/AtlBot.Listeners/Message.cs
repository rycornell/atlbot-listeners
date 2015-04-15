using System;

namespace AtlBot.Listeners
{
    public class Message : IMessage
    {
        public event EventHandler<ResponseRequestedEventArgs> ResponseRequested;

        public string Text { get; set; }

        public void Respond(IResponse response)
        {
            if (ResponseRequested != null)
                ResponseRequested(this, new ResponseRequestedEventArgs { Response = response });
        }
    }

    public class ResponseRequestedEventArgs
    {
        public IResponse Response { get; set; }
    }
}
