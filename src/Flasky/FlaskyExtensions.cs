using System;
using Owin;

namespace Flasky
{
    public static class FlaskyExtensions
    {
        public static void UseFlasky(this IAppBuilder appBuilder, Action<FlaskyConfiguration> configAction)
        {
            var appConfig = new FlaskyConfiguration();
            configAction(appConfig);
            appConfig.Initialise(appBuilder);
        }
    }
}
