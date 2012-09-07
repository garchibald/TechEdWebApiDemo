//
// Grant Archibald (c) 2012
//
// See Licence.txt
//

using System.Web.Http;
using TechEdWebApiDemo.Models;

namespace TechEdWebApiDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SetupODataRoutes(config);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        /// <summary>
        /// Setup OData http://blogs.msdn.com/b/alexj/archive/2012/08/15/odata-support-in-asp-net-web-api.aspx
        /// </summary>
        /// <param name="config"></param>
        private static void SetupODataRoutes(System.Web.Http.HttpConfiguration config)
        {
            var modelBuilder = new System.Web.Http.OData.Builder.ODataConventionModelBuilder();

            modelBuilder.EntitySet<Member>("Members");

            var model = modelBuilder.GetEdmModel();

            // Create the OData formatter and give it the model
            var odataFormatter = new System.Web.Http.OData.Formatter.ODataMediaTypeFormatter(model);

            // Register the OData formatter
            config.Formatters.Insert(0, odataFormatter);

            //Next you need to setup some routes to handle common OData requests, below are the routes required for a Read/Write OData model built using the OData Routing conventions that also supports client side code-generation (vital if you want a WCF DS client application to talk to your service).

            // Metadata routes to support $metadata and code generation in the WCF Data Service client.
            config.Routes.MapHttpRoute(
                System.Web.Http.OData.Builder.Conventions.ODataRouteNames.Metadata,
                "api/$metadata",
                new {Controller = "ODataMetadata", Action = "GetMetadata"}
                );
            config.Routes.MapHttpRoute(
                System.Web.Http.OData.Builder.Conventions.ODataRouteNames.ServiceDocument,
                "api",
                new {Controller = "ODataMetadata", Action = "GetServiceDocument"}
                );

            // Relationship routes (notice the parameters is {type}Id not id, this avoids colliding with GetById(id)).
            // This code handles requests like ~/ProductFamilies(1)/Products
            config.Routes.MapHttpRoute(System.Web.Http.OData.Builder.Conventions.ODataRouteNames.PropertyNavigation,
                                       "api/{controller}({parentId})/{navigationProperty}");

            // Route for manipulating links, the code allows people to create and delete relationships between entities
            config.Routes.MapHttpRoute(System.Web.Http.OData.Builder.Conventions.ODataRouteNames.Link,
                                       "api/{controller}({id})/$links/{navigationProperty}");

            // Routes for urls both producing and handling urls like ~/Product(1), ~/Products() and ~/Products      
            config.Routes.MapHttpRoute(System.Web.Http.OData.Builder.Conventions.ODataRouteNames.GetById,
                                       "api/{controller}({id})");
            config.Routes.MapHttpRoute(
                System.Web.Http.OData.Builder.Conventions.ODataRouteNames.DefaultWithParentheses, "api/{controller}()");
            config.Routes.MapHttpRoute(System.Web.Http.OData.Builder.Conventions.ODataRouteNames.Default,
                                       "api/{controller}");
        }
    }
}
