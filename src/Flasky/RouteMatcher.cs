using System;
using System.Collections.Generic;
using Owin.Types;
using System.Linq;

namespace Flasky
{
    public class RouteMatcher
    {
        private readonly IDictionary<RouteBase, Func<OwinRequest, object>> _routeHandlers;

        public RouteMatcher(IDictionary<RouteBase, Func<OwinRequest, object>> routeHandlers)
        {
            _routeHandlers = routeHandlers;
        }

        public bool HasMatch(OwinRequest request)
        {
            return _routeHandlers.Any(rvp => rvp.Key.HasMatch(request));
        }

        public Func<OwinRequest, object> GetMatch(OwinRequest request)
        {
            var match = _routeHandlers
                        .Where(rvp => rvp.Key.HasMatch(request))
                        .OrderByDescending(rvp => rvp.Key.Specificity)
                        .Select(rvp => rvp.Value).FirstOrDefault();
            return match;
        }
    }
}