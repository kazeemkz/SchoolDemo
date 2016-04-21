using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace SchoolManagement
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();

            //jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();


            config.Routes.MapHttpRoute(
             name: "CommentsRoute",
             routeTemplate: "api/posts/{postsid}/Comments/{id}",
             defaults: new { controller = "Comments", id = RouteParameter.Optional }
         );

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
              name: "DefaultApi",
              routeTemplate: "api/posts/{id}",
              defaults: new { controller = "posts", id = RouteParameter.Optional }
          );

            // var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            // config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);



            //var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            //jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var json = config.Formatters.JsonFormatter;

            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter); //new CamelCasePropertyNamesContractResolver();

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();
        }
    }
}