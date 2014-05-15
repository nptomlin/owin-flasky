using System;
using Microsoft.Owin.Hosting;
using Owin;

namespace Flasky.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            string uri = "http://localhost:8080/";

            using (WebApp.Start(uri, Configure))
            {
                Console.WriteLine("Started");
                Console.ReadKey();
                Console.WriteLine("Stopping");
            }
        }

        public static void Configure(IAppBuilder app)
        {
            app.UseFlasky(appConfig =>
                              {
                                  appConfig.AddRouteHandler("/", RouteHandlers.Welcome)
                                           .AddRouteHandler("/foo", RouteHandlers.WelcomeAgain)
                                           .AddRouteHandler("/jsonfoo", RouteHandlers.WelcomeJson);
                              });
        }
    }


}
