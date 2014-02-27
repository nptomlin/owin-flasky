using Owin.Types;

namespace Flasky.Sample
{
    public static class RouteHandlers
    {
        public static object Welcome(OwinRequest request)
        {
            return "Hello Mum";
        }

        public static object WelcomeAgain(OwinRequest request)
        {
            return "Hello Again";
        }
    }
}