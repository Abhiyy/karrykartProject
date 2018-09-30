using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KarryKart.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var corsAttr = new EnableCorsAttribute("http://localhost:15557", "*", "*");
            config.EnableCors(corsAttr);
            config.EnableCors();

            var corsAttr1 = new EnableCorsAttribute("http://localhost:8100", "*", "*");
            config.EnableCors(corsAttr1);

            //var corsAttr2 = new EnableCorsAttribute("http://hg.karrykart.com", "*", "*");
            //config.EnableCors(corsAttr2);
            config.EnableCors();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               name: "DefaultApiWithAction",
               routeTemplate: "api/{controller}/{action}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );

           
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

           
        }
    }
}
