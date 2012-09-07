//
// Grant Archibald (c) 2012
//
// See Licence.txt
//

using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Http.Filters;

namespace Resources.Security
{
    public class SecurityExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is SecurityException)
            {
                // log exception details

                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                response.Content = new StringContent(actionExecutedContext.Exception.Message);

                actionExecutedContext.Response = response;
            }

        }
    }
}
