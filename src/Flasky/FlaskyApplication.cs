using Owin;

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
                                                   res.ContentType = "text/plain";
                                                   return res.WriteAsync(_routeMatcher.GetMatch(req)(req).ToString());
                                               }
                                               res.StatusCode = 404;
                                               res.ContentType = "text/plain";
                                               return res.WriteAsync("Not found");
                                           });
        }
    }
}