using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ServiceStack;

namespace Flasky
{
    public class RegexRoute : RouteBase
    {
        static readonly Regex PathParseRegex;

        static RegexRoute()
        {
            //stolen from flask
            const string pattern = @"(?<static>[^<]*) # static rule data
<
(?:
(?<converter>[a-zA-Z_][a-zA-Z0-9_]*) # converter name
(?:\((?<args>.*?)\))? # converter arguments
\: # variable delimiter
)?
(?<variable>[a-zA-Z_][a-zA-Z0-9_]*) # variable name
>
";

            PathParseRegex = new Regex(pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
        }


        public RegexRoute(string path) : base(path)
        {
            
        }

        public RegexRoute(string path, string method) : base(path, method)
        {
        }

        public override bool HasMatch(string path)
        {
            var match = PathParseRegex.Match(path);
            return match.Success;
        }

        /// <summary>
        /// Aka parse rule.. prepares components for matcher
        /// </summary>
        private IEnumerable<Tuple<string, string>> Prepare()
        {
            //Parse a rule and return it as generator. Each iteration yields tuples
            //in the form ``(converter, arguments, variable)``. If the converter is
            //`None` it's a static url part, otherwise it's a dynamic one.

            var pos = 0;
            var end = Path.Length;
            Func<string, int, Match> do_match = PathParseRegex.Match;
            var usedNames = new HashSet<String>();
            while (pos < end)
            {
                var m = do_match(Path, pos);
                if(!m.Success)
                {
                    break;
                }
                var data = m.Groups;
                if(data["static"].Success)
                {
                    yield return new Tuple<string, string>(null, data["static"].Value);
                }
                var variable = data["variable"].Value;
                //var converter = data['converter'] or 'default' // converter not required ?? 
                if(usedNames.Contains(variable))
                {
                    throw new ArgumentException("variable name {0} used twice.".Fmt(variable), Path);// perhaps a route config error
                }
                usedNames.Add(variable);
                yield return new Tuple<string, string>(data["args"].Success ? data["args"].Value : null,variable);
                pos = m.Index + m.Length;
            }
            if( pos < end)
            {
                var remaining = Path.Substring(pos);
                if(remaining.Contains(">") || remaining.Contains("<"))
                {
                    throw new ArgumentException("'malformed url rule: {0}".Fmt(Path), Path);
                }
                yield return new Tuple<string, string>(null, remaining);
            }

        }
    }
}