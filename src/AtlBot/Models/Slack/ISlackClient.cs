using System.Collections.Specialized;

namespace AtlBot.Models.Slack
{
    public interface ISlackClient
    {
        string Invoke(string apiMethod, NameValueCollection data);
    }
}
