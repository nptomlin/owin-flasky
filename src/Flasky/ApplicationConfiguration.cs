using System;
using System.Collections.Generic;
using Owin;
using Owin.Types;

namespace Flasky
{
    public class ApplicationConfiguration
    {
        private readonly IDictionary<string, Func<OwinRequest, object>> _routeHandlers;

        public ApplicationConfiguration()
        {
            _routeHandlers = new Dictionary<string, Func<OwinRequest, object>>();
        }

        public ApplicationConfiguration RegisterRouteHandler(string path, Func<OwinRequest, object> handler)
        {
            _routeHandlers.Add(path, handler);
            return this;
        }

        protected internal void Initialise(IAppBuilder appBuilder)
        {
            appBuilder.UseHandlerAsync((req, res) =>
            {
                if(_routeHandlers.ContainsKey(req.Path))
                {
                    res.ContentType = "text/plain";
                    return res.WriteAsync(_routeHandlers[req.Path](req).ToString());
                }
                res.StatusCode = 404;
                res.ContentType = "text/plain";
                return res.WriteAsync("Not found");
            });
        }
    }
}