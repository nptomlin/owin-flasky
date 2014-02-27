using System;
using System.Collections.Generic;
using Owin;
using Owin.Types;

namespace Flasky
{
    public class FlaskyConfiguration
    {
        private readonly IDictionary<string, Func<OwinRequest, object>> _routeHandlers;

        public FlaskyConfiguration()
        {
            _routeHandlers = new Dictionary<string, Func<OwinRequest, object>>();
        }

        public FlaskyConfiguration RegisterRouteHandler(string path, Func<OwinRequest, object> handler)
        {
            _routeHandlers.Add(path, handler);
            return this;
        }

        protected internal void Initialise(IAppBuilder appBuilder)
        {
            var flaskyApp = new FlaskyApplication(new RouteMatcher(_routeHandlers));
            flaskyApp.Initialise(appBuilder);
        }
    }
}