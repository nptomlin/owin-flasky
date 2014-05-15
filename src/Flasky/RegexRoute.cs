using System.Text.RegularExpressions;

namespace Flasky
{
    public class RegexRoute : RouteBase
    {
        public RegexRoute(string path) : base(path)
        {
        }

        public RegexRoute(string path, string method) : base(path, method)
        {
        }

        public override bool HasMatch(string path)
        {
            var regex = new Regex(Path);
            var match = regex.Match(path);
            return match.Success;
        }
    }
}