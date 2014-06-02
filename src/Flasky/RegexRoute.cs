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
        
        Regex _pathMatchRegex;

        public RegexRoute(string path) : base(path)
        {
            _isLeaf = !path.EndsWith("/");
        }

        public RegexRoute(string path, string method) : base(path, method)
        {
            _isLeaf = !path.EndsWith("/");
        }

        public override bool HasMatch(string path)
        {
            if(_pathMatchRegex == null)
            {
                lock (_syncLock)
                {
                    if(_pathMatchRegex == null)
                    {
                        Compile();
                    }
                }
            }
            var match = _pathMatchRegex.Match(path);
            return match.Success;
        }

        private void Compile()
        {
            //def compile(self):
            //"""Compiles the regular expression and stores it."""
            //assert self.map is not None, 'rule not bound'

            //if self.map.host_matching:
            //    domain_rule = self.host or ''
            //else:
            //    domain_rule = self.subdomain or ''

            //self._trace = []
            //self._converters = {}
            //self._weights = []
            //regex_parts = []
            var regexParts = new List<string>();
            foreach(var tuple in PathParseHelper.Prepare(_isLeaf ? Path : Path.TrimEnd('/')))
            {
                var converter = tuple.Item1;
                var variable = tuple.Item2;
                if (converter == null)
                {
                    regexParts.Add(Regex.Escape(variable));
                }
                else if (converter.Equals("default"))
                {
                    regexParts.Add("(?<{0}>[^/]+)".Fmt(variable));
                }
                else if (converter.Equals("wildcard"))
                {
                    regexParts.Add("(?<{0}>[^/].*?)".Fmt(variable));
                }

            }

            if(!_isLeaf)
            {
                regexParts.Add("(?<!/)(?<__suffix__>/?)");
            }

            _pathMatchRegex = new Regex("^{0}$".Fmt(regexParts.Join("")), RegexOptions.Compiled | RegexOptions.IgnoreCase);

            //def _build_regex(rule):
            //    for converter, arguments, variable in parse_rule(rule):
            //        if converter is None:
            //            regex_parts.append(re.escape(variable))
            //            self._trace.append((False, variable))
            //            for part in variable.split('/'):
            //                if part:
            //                    self._weights.append((0, -len(part)))
            //        else:
            //            if arguments:
            //                c_args, c_kwargs = parse_converter_args(arguments)
            //            else:
            //                c_args = ()
            //                c_kwargs = {}
            //            convobj = self.get_converter(
            //                variable, converter, c_args, c_kwargs)
            //            regex_parts.append('(?P<%s>%s)' % (variable, convobj.regex))
            //            self._converters[variable] = convobj
            //            self._trace.append((True, variable))
            //            self._weights.append((1, convobj.weight))
            //            self.arguments.add(str(variable))

            //_build_regex(domain_rule)
            //regex_parts.append('\\|')
            //self._trace.append((False, '|'))
            //_build_regex(self.is_leaf and self.rule or self.rule.rstrip('/'))
            //if not self.is_leaf:
            //    self._trace.append((False, '/'))

            //if self.build_only:
            //    return
            //regex = r'^%s%s$' % (
            //    u''.join(regex_parts),
            //    (not self.is_leaf or not self.strict_slashes) and
            //        '(?<!/)(?P<__suffix__>/?)' or ''
            //)
            //self._regex = re.compile(regex, re.UNICODE)
        }
    }
}