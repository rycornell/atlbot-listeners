using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using SimpleOAuth;

namespace AtlBot.Listeners
{
    public class RestaurantListener : IMessageListener
    {
        public string MessagePattern
        {
            get { return @"restaurant( \d{5})?( \d{2})?"; }
        }

        public string HelpText
        {
            get { return "restaurant [zip code] [radius in miles] - displays a random restaurant for the given zip code and radius"; }
        }

        public void HandleMessage(IMessage message)
        {
            var match = Regex.Match(message.Text, MessagePattern, RegexOptions.IgnoreCase);
            if (!match.Success) return;

            const string term = "restaurant";
            const int searchLimit = 20;

            var location = "30309";
            if (match.Groups[1].Success)
            {
                location = match.Groups[1].Value;
            }

            var radiusInMiles = 10;
            if (match.Groups[2].Success)
            {
                radiusInMiles = int.Parse(match.Groups[2].Value);
            }
            var radiusInMeters = ConvertMilesToMeters(radiusInMiles);

            var businesses = new JArray();

            var page = 0;
            while (page < 3) // get 60 results
            {
                var queryParams = new NameValueCollection
                {
                    {"term", term},
                    {"location", location},
                    {"limit", searchLimit.ToString()},
                    {"radius_filter", radiusInMeters.ToString()},  // meters 
                    {"offset", (page*searchLimit).ToString()}
                };

                var businessResults = (JArray) (SearchYelp(queryParams).GetValue("businesses"));
                foreach (var result in businessResults)
                {
                    businesses.Add(result);
                }

                page++;
            }

            string responseText;
            if (businesses.Count == 0)
            {
                responseText = String.Format("No businesses for {0} in {1} found.", term, location);
            }
            else
            {
                var index = new Random().Next(0, businesses.Count);
                var business = businesses[index];
                var url = business["url"];
                responseText = "<" + url + ">"; // make into a slack link
            }

            message.Respond(new Response
            {
                Text = responseText
            });
        }

        private double ConvertMilesToMeters(int miles)
        {
            return miles*1609.344;
        }

        private JObject SearchYelp(NameValueCollection parameters)
        {
            const string baseUrl = "http://api.yelp.com";
            const string urlPath = "/v2/search/";

            // get keys here https://www.yelp.com/developers/manage_api_keys
            const string consumerKey = "";
            const string consumerSecret = "";
            const string accessToken = "";
            const string accessTokenSecret = "";

            var queryStringValues = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryStringValues.Add(parameters);
            
            var uriBuilder = new UriBuilder(baseUrl + urlPath)
            {
                Query = queryStringValues.ToString()
            };

            var request = WebRequest.Create(uriBuilder.ToString());
            request.Method = "GET";

            request.SignRequest(new Tokens
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                AccessToken = accessToken,
                AccessTokenSecret = accessTokenSecret
            })
            .WithEncryption(EncryptionMethod.HMACSHA1)
            .InHeader();

            var response = request.GetResponse();
            var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            var data = stream.ReadToEnd();
            return JObject.Parse(data);
        }
    }
}