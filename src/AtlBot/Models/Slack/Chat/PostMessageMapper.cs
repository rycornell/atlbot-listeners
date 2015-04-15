using System;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace AtlBot.Models.Slack.Chat
{
    /// <summary>
    /// Map custom types to/from Slack API types
    /// See https://api.slack.com/methods/chat.postMessage
    /// </summary>
    public static class PostMessageMapper
    {
        public static NameValueCollection Map(PostMessageRequest request)
        {
            var collection = new NameValueCollection
            {
                {"token", request.Token},
                {"channel", request.Channel}, 
                {"text", request.Text}
            };

            // optional fields
            if (!String.IsNullOrWhiteSpace(request.Username))
            {
                collection.Add("username", request.Username);
            }

            if (!String.IsNullOrWhiteSpace(request.IconUrl))
            {
                collection.Add("icon_url", request.Username);
            }

            if (request.UnfurlLinks.HasValue)
            {
                collection.Add("unfurl_links", request.UnfurlLinks.Value.ToString().ToLower());
            }

            if (request.AsUser.HasValue)
            {
                collection.Add("as_user", request.AsUser.Value.ToString().ToLower());
            }

            return collection;
        }

        public static PostMessageResponse Map(string response)
        {
            var postMessageResponse = new PostMessageResponse();

            var json = JObject.Parse(response);

            postMessageResponse.Ok = json.Value<bool>("ok");
            postMessageResponse.Error = json.Value<string>("error");
            postMessageResponse.Timestamp = json.Value<string>("ts");
            postMessageResponse.ChannelId = json.Value<string>("channel");

            return postMessageResponse;
        }
    }
}