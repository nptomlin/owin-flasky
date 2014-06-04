using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
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
            _routeHandlers.Add(new RegexRoute(path), handler);
            return this;
        }

        public FlaskyConfiguration AddRouteHandler(string path, string method, Func<OwinRequest, object> handler)
        {
            _routeHandlers.Add(new RegexRoute(path, method), handler);
            return this;
        }

        public FlaskyConfiguration AddRouteHandler(string path, Type type, string method)
        {
            //this is an experiment
            var regexRoute = new RegexRoute(path);
            Func<OwinRequest, object> handler = (req) =>
                                            {
                                                var typeMethod = type.GetMethod(method);
                                                var typeArgs = typeMethod.GetParameters();
                                                var arguments = new List<object>();
                                                foreach (var parameterInfo in typeArgs)
                                                {
                                                    if(parameterInfo.ParameterType == typeof (OwinRequest))
                                                    {
                                                        arguments.Add(req);
                                                    }
                                                    else
                                                    {
                                                        arguments.Add(regexRoute.GetParameterValue(req.Path, parameterInfo.Name));
                                                    }
                                                }
                                                return typeMethod.Invoke(type, arguments.ToArray());
                                            };
            _routeHandlers.Add(regexRoute, handler);
            return this;
        }


        protected internal IAppBuilder Initialise(IAppBuilder appBuilder)
        {
            return appBuilder.Use(typeof(FlaskyApplication), new RouteMatcher(_routeHandlers));
        }
    }
}