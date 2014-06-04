using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ServiceStack;

namespace Flasky
{
    public class RegexRoute : RouteBase
    {
        readonly bool _isLeaf;
        readonly object _syncLock = new object();
        readonly List<string> _parameters;

        Regex _pathMatchRegex;

        public RegexRoute(string path) : base(path)
        {
            _parameters = new List<string>();
            _isLeaf = !path.EndsWith("/");
        }

        public RegexRoute(string path, string method) : base(path, method)
        {
            _parameters = new List<string>();
            _isLeaf = !path.EndsWith("/");
        }

        public string[] Parameters
        {
            get
            { 
                Initialise();
                return _parameters.ToArray();
            }
        }

        public object GetParameterValue(string path, string param)
        {
            Initialise();
            var match = _pathMatchRegex.Match(path);
            return match.Groups[param].Success ? match.Groups[param].Value : null;
        }

        public override bool HasMatch(string path)
        {
            Initialise();
            var match = _pathMatchRegex.Match(path);
            return match.Success;
        }

        private void Initialise()
        {
            if (_pathMatchRegex == null)
            {
                lock (_syncLock)
                {
                    if (_pathMatchRegex == null)
                    {
                        Compile();
                    }
                }
            }
        }

        private void Compile()
        {
            var regexParts = new List<string>();
            foreach(var tuple in PathParseHelper.Prepare(_isLeaf ? Path : Path.TrimEnd('/')))
            {
                var converter = tuple.Item1;
                var variable = tuple.Item2;
                switch (converter)
                {
                    case null:
                        regexParts.Add(Regex.Escape(variable));
                        break;
                    case "default":
                        regexParts.Add("(?<{0}>{1})".Fmt(variable, "[^/]+"));
                        _parameters.Add(variable);
                        break;
                    case "path":
                        regexParts.Add("(?<{0}>{1})".Fmt(variable, "[^/].*?"));//[^/].*?
                        _parameters.Add(variable);
                        break;
                    case "int":
                        regexParts.Add("(?<{0}>{1})".Fmt(variable, "\\d+"));
                        _parameters.Add(variable);
                        break;
                }
            }

            if(!_isLeaf)
            {
                regexParts.Add("(?<!/)(?<__suffix__>/?)");
            }

            _pathMatchRegex = new Regex("^{0}$".Fmt(regexParts.Join("")), RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
    }
}