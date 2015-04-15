using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using AtlBot.Listeners;
using AtlBot.Models;

namespace AtlBot
{
    public class ListenerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var listeners = Assembly.GetAssembly(typeof(IMessageListener))
                .GetTypes()
                .Where(p => p.IsClass && typeof(IMessageListener).IsAssignableFrom(p));

            foreach (var listener in listeners)
            {
                ListenerRegistration.Listeners.Add(Activator.CreateInstance(listener) as IMessageListener);
            }
        }
    }
}