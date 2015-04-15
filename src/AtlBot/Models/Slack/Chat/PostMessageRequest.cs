namespace AtlBot.Models.Slack.Chat
{
    public class PostMessageRequest : RequestBase
    {
        public string Channel { get; set; }

        public string Username { get; set; }

        public string Text { get; set; }

        public string IconUrl { get; set; }

        public bool? UnfurlLinks { get; set; }

        public bool? AsUser { get; set; }
    }
}