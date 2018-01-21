using System;
using System.IO;
using System.Web;
using System.Web.Routing;

namespace GIR.Intranet.Infraestructure.Extensions
{
    public static class UriExtensions
    {
        public static RouteValueDictionary GetRouteData(this Uri uri)
        {
            // Split the url to url + query string
            var fullUrl = uri.ToString();
            var questionMarkIndex = fullUrl.IndexOf('?');
            string queryString = null;
            string url = fullUrl;
            if (questionMarkIndex != -1) // There is a QueryString
            {
                url = fullUrl.Substring(0, questionMarkIndex);
                queryString = fullUrl.Substring(questionMarkIndex + 1);
            }

            // Arranges
            var request = new HttpRequest(null, url, queryString);
            var response = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(request, response);

            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            // Extract the data    
            var values = routeData.Values;
            var controllerName = values["controller"];
            var actionName = values["action"];
            var areaName = values["area"];

            return new RouteValueDictionary(new
            {
                controller = controllerName,
                action = actionName,
                area = areaName
            });
        }
    }
}