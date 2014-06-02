using System;
using Owin;

namespace Flasky
{
    public static class FlaskyExtensions
    {
        public static IAppBuilder UseFlasky(this IAppBuilder appBuilder, Action<FlaskyConfiguration> configAction)
        {
            var appConfig = new FlaskyConfiguration();
            configAction(appConfig);
            return appConfig.Initialise(appBuilder);
        }
    }
}
