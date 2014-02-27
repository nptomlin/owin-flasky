using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                                  appConfig.RegisterRouteHandler("/", RouteHandlers.Welcome)
                                           .RegisterRouteHandler("/foo", RouteHandlers.WelcomeAgain);
                              });
        }
    }


}
