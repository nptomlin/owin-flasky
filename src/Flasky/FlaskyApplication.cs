using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flasky.Response;
using Owin.Types;


namespace Flasky
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class FlaskyApplication
    {
        readonly RouteMatcher _routeMatcher;
        readonly AppFunc _next;

        public FlaskyApplication(AppFunc next, RouteMatcher routeMatcher)
        {
            _next = next;
            _routeMatcher = routeMatcher;
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            var request = new OwinRequest(environment);
            if (_routeMatcher.HasMatch(request))
            {
                var response = new OwinResponse(environment);
		        var routeHandler = _routeMatcher.GetMatch(request);
                return InvokeHandler(response, routeHandler, request);
            }
            return _next(environment);
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