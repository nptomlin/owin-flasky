using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                response.ContentType = "text/plain";
                return response.WriteAsync(_routeMatcher.GetMatch(request)(request).ToString());
            }
            return _next(environment);
        }

    }
}