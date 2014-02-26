using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;

namespace Flasky
{
    public static class FlaskyExtensions
    {
        public static void UseFlasky(this IAppBuilder appBuilder, Action<ApplicationConfiguration> configAction)
        {
            var appConfig = new ApplicationConfiguration();
            configAction(appConfig);
            appConfig.Initialise(appBuilder);
        }
    }
}
