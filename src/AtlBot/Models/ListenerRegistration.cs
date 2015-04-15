using System.Collections.Generic;
using AtlBot.Listeners;

namespace AtlBot.Models
{
    public static class ListenerRegistration
    {
        public static List<IMessageListener> Listeners = new List<IMessageListener>();
    }
}