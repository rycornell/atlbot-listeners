namespace AtlBot.Models.Slack
{
    public abstract class ResponseBase
    {
        public bool Ok { get; set; }

        public string Error { get; set; }
    }
}