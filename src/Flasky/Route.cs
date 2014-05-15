namespace Flasky
{
    public class Route : RouteBase
    {
        public Route(string path) : base(path)
        {
        }

        public Route(string path, string method) : base(path, method)
        {
        }

        public override bool HasMatch(string path)
        {
            return path == Path;
        }
    }
}