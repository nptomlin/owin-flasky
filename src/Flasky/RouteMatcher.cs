using System;
using System.Collections.Generic;
using Owin.Types;

namespace Flasky
{
    public class RouteMatcher
    {
        private readonly IDictionary<string, Func<OwinRequest, object>> _routeHandlers;

        public RouteMatcher(IDictionary<string, Func<OwinRequest, object>> routeHandlers)
        {
            _routeHandlers = routeHandlers;
        }

        public bool HasMatch(OwinRequest request)
        {
            return _routeHandlers.ContainsKey(request.Path);
        }

        public Func<OwinRequest, object> GetMatch(OwinRequest request)
        {
            return _routeHandlers[request.Path];
        }
    }
}