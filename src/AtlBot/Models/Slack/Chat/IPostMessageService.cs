namespace AtlBot.Models.Slack.Chat
{
    public interface IPostMessageService
    {
        PostMessageResponse PostMessage(PostMessageRequest request);
    }
}
