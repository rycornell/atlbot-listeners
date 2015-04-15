﻿using System.Web.Http;

namespace AtlBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(ListenerConfig.Register);
        }
    }
}
