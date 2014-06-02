using System;
using System.Collections.Generic;
using Owin;
using Owin.Types;

namespace Flasky
{
    public class FlaskyConfiguration
    {
        private readonly IDictionary<RouteBase, Func<OwinRequest, object>> _routeHandlers;

        public FlaskyConfiguration()
        {
            _routeHandlers = new Dictionary<RouteBase, Func<OwinRequest, object>>();
        }

        public FlaskyConfiguration AddRouteHandler(string path, Func<OwinRequest, object> handler)
        {
            _routeHandlers.Add(new Route(path), handler);
            return this;
        }

        public FlaskyConfiguration AddRouteHandler(string path, string method, Func<OwinRequest, object> handler)
        {
            _routeHandlers.Add(new Route(path, method), handler);
            return this;
        }

        protected internal IAppBuilder Initialise(IAppBuilder appBuilder)
        {
            return appBuilder.Use(typeof(FlaskyApplication), new RouteMatcher(_routeHandlers));
        }
    }
}