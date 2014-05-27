using System;
using System.Threading.Tasks;
using Flasky.Response;
using Owin;
using Owin.Types;

namespace Flasky
{
    public class FlaskyApplication
    {
        readonly RouteMatcher _routeMatcher;

        public FlaskyApplication(RouteMatcher routeMatcher)
        {
            _routeMatcher = routeMatcher;
        }

        protected internal void Initialise(IAppBuilder appBuilder)
        {
            appBuilder.UseHandlerAsync((req, res) =>
                                           {
                                               if (_routeMatcher.HasMatch(req))
                                               {
                                                   var routeHandler = _routeMatcher.GetMatch(req);
                                                   return InvokeHandler(res, routeHandler, req);
                                               }
                                               res.StatusCode = 404;
                                               res.ContentType = "text/plain";
                                               return res.WriteAsync("Not found");
                                           });
        }

        private static Task InvokeHandler(OwinResponse res, Func<OwinRequest, object> routeHandler, OwinRequest req)
        {
            var result = routeHandler(req);
            var flaskyResponse = result as IResponse;
            if(flaskyResponse != null)
            {
                return flaskyResponse.Write(res);
            }
            res.ContentType = "text/plain";
            return res.WriteAsync(result.ToString());
        }
    }
}