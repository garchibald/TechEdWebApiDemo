//
// Grant Archibald (c) 2012
//
// See Licence.txt
//

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http.OData;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Query;
using Microsoft.Data.Edm;

namespace OData.Framework
{
    public static class ODataExtensions
    {
        /// <summary>
        /// Parse the <see cref="Request"/> to map non standard OData queries into OData queries
        /// </summary>
        /// <remarks>Examples of non standard query</remarks>
        /// <remarks>http://example.com/movies?rating=G,PG</remarks>
        /// <remarks>http://example.com/movies?id=1,2,3</remarks>
        /// <remarks>http://example.com/movies?released=2010-11-01</remarks>
        /// <typeparam name="T">The type that the query relates to</typeparam>
        /// <param name="query">The query to parse</param>
        /// <param name="request">The request to parse</param>
        /// <returns>The parse query</returns>
        public static ODataQueryOptions Parse<T>(
            this ODataQueryOptions query, HttpRequestMessage request) where T : class
        {
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<T>(typeof (T).Name + "s");
            var model = modelBuilder.GetEdmModel();

            return
                new ODataQueryOptions(
                    new ODataQueryContext(model, typeof (T)), MapQueryRequest(model, request));

        }

        private static HttpRequestMessage MapQueryRequest(IEdmModel model,
                                                          HttpRequestMessage request)
        {
            var uri = string.Format("{0}://{1}/{2}?{3}", request.RequestUri.Scheme
                                    ,
                                    request.RequestUri.GetComponents(UriComponents.HostAndPort,
                                                                     UriFormat.SafeUnescaped)
                                    ,
                                    request.RequestUri.GetComponents(UriComponents.Path,
                                                                     UriFormat.SafeUnescaped)
                                    , ParseQuery(request.RequestUri.ParseQueryString(), model));
            var mappedUri = new Uri(uri);

            var mappedRequest = new HttpRequestMessage {RequestUri = mappedUri};
            return mappedRequest;
        }

        private static string ParseQuery(NameValueCollection queryNames,
                                         IEdmModel model)
        {
            var query = new StringBuilder();
            foreach (var key in queryNames.AllKeys)
            {
                if (query.Length > 0)
                    query.Append("&");
                if (key.StartsWith("$"))
                {
                    query.Append(key);
                    query.Append("=");
                    query.Append(queryNames[key]);
                }
                else
                {
                    if (!string.IsNullOrEmpty(queryNames[key]))
                    {
                        query.Append("$filter=");
                        query.Append("(");
                        for (int index = 0;
                             index <
                             queryNames[key].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Length;
                             index++)
                        {
                            if (index > 0)
                            {
                                query.Append(" or ");
                            }
                            var keyPart =
                                queryNames[key].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)[index];

                            var prop = FindEdmProperty(model, key);

                            query.Append(prop != null ? prop.Name : key);

                            query.Append(" eq ");
                            if (prop == null || prop.Type.IsString())
                                query.Append("'");

                            if (prop == null || prop.Type.IsDateTime())
                                query.Append("datetime'");

                            query.Append(keyPart);

                            if (prop == null || prop.Type.IsDateTime() || prop.Type.IsString())
                                query.Append("'");
                        }
                        query.Append(")");
                    }

                }
            }
            return query.ToString();
        }

        private static IEdmProperty FindEdmProperty(IEdmModel model,
                                                    string keyPart)
        {
            if (model == null)
                return null;

            if (model.EntityContainers() == null)
                return null;

            if (!model.EntityContainers().Any())
                return null;

            var container = model.EntityContainers().First();
            if (!container.EntitySets().Any())
                return null;
            var entity = container.EntitySets().First();

            if (entity.ElementType == null)
                return null;

            var elementType = entity.ElementType;

            return
                elementType.Properties().FirstOrDefault(
                    p => string.Compare(p.Name, keyPart, StringComparison.OrdinalIgnoreCase) == 0);
        }
    }
}