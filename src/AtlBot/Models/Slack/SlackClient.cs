using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;

namespace AtlBot.Models.Slack
{
    public class SlackClient : ISlackClient
    {
        private const string Host = "https://slack.com/api/";

        public string Invoke(string apiMethod, NameValueCollection parameters)
        {
            var baseUri = new Uri(Host);
            var apiUri = new Uri(baseUri, apiMethod);

            // crazy happens
            if (ConfigurationManager.AppSettings["SafeMode"] == "On") return "safe mode";

            using (var client = new WebClient())
            {
                var response = client.UploadValues(apiUri, parameters);
                return System.Text.Encoding.UTF8.GetString(response);
            }
        }
    }
}