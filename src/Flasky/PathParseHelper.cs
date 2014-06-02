using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ServiceStack;

namespace Flasky
{
    public static class PathParseHelper
    {
        private static readonly Regex PathParseRegex;

        static PathParseHelper()
        {
            //stolen from flask
            const string pattern = @"(?<static>[^<]*) # static rule data
<
(?<variable>[a-zA-Z_][a-zA-Z0-9_]*) # variable name
(?<wildcard>[\*]{0,1}) # wildcard
>
";

            const string orig_pattern = @"(?<static>[^<]*) # static rule data
<
(?:
(?<converter>[a-zA-Z_][a-zA-Z0-9_]*) # converter name
\: # variable delimiter
)?
(?<variable>[a-zA-Z_][a-zA-Z0-9_]*) # variable name
>
";

            var v = @"(?P<static>[^<]*) # static rule data
<
(?:
(?P<converter>[a-zA-Z_][a-zA-Z0-9_]*) # converter name
(?:\((?P<args>.*?)\))? # converter arguments
\: # variable delimiter
)?
(?P<variable>[a-zA-Z_][a-zA-Z0-9_]*) # variable name
>";
            PathParseRegex = new Regex(pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        }

        /// <summary>
        /// Aka parse rule.. prepares components for matcher
        /// </summary>
        /// <param name="path"> </param>
        public static IEnumerable<Tuple<string, string>> Prepare(string path)
        {
            //Parse a rule and return it as generator. Each iteration yields tuples
            //in the form ``(converter, arguments, variable)``. If the converter is
            //`None` it's a static url part, otherwise it's a dynamic one.

            var pos = 0;
            var end = path.Length;
            Func<string, int, Match> do_match = PathParseRegex.Match;
            var usedNames = new HashSet<String>();
            while (pos < end)
            {
                var m = do_match(path, pos);
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
                var wildcard = data["wildcard"].Success && !String.IsNullOrEmpty(data["wildcard"].Value);
                if(usedNames.Contains(variable))
                {
                    throw new ArgumentException("variable name {0} used twice.".Fmt(variable), path);// perhaps a route config error
                }
                usedNames.Add(variable);
                yield return new Tuple<string, string>(wildcard ? "wildcard" : "default" ,variable);
                pos = m.Index + m.Length;
            }
            if( pos < end)
            {
                var remaining = path.Substring(pos);
                if(remaining.Contains(">") || remaining.Contains("<"))
                {
                    throw new ArgumentException("'malformed url rule: {0}".Fmt(path), path);
                }
                yield return new Tuple<string, string>(null, remaining);
            }
        }
    }
}