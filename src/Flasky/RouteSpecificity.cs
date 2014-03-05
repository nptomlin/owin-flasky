namespace Flasky
{
    public static class RouteSpecificity
    {
        public static int None = 0;
        public static int Path = 1;
        public static int Method = 1;
        public static int Accept = 1;
        public static int PathAndMethod = Path + Method;
        public static int PathAndAccept = Path + Accept;
        public static int PathAndMethodAndAccept = Path + Method + Accept;
    }
}