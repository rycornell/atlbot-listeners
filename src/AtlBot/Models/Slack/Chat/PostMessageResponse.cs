namespace AtlBot.Models.Slack.Chat
{
    public class PostMessageResponse : ResponseBase
    {
        public string Timestamp { get; set; }

        public string ChannelId { get; set; }
    }
}