namespace AtlBot.Models.Slack.Chat
{
    public class PostMessageService : IPostMessageService
    {
        private readonly ISlackClient _client;

        public PostMessageService(ISlackClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Posts a message to Slack
        /// See https://api.slack.com/methods/chat.postMessage
        /// </summary>
        /// <param name="request"></param>
        public PostMessageResponse PostMessage(PostMessageRequest request)
        {
            var parameters = PostMessageMapper.Map(request);
            var response = _client.Invoke("chat.postMessage", parameters);
            return PostMessageMapper.Map(response);
        }
    }
}