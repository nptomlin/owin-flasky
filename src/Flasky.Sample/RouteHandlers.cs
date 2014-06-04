using System;
using Flasky.Response;
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

        public static object WelcomeJson(OwinRequest request)
        {
            return new {foo = "bar"}.AsJsonResponse();
        }

        public static object HelloMum(OwinRequest arg, string one, string two)
        {
            return new { foo = "bar", one, two }.AsJsonResponse();
        }
    }
}