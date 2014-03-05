using System;
using Owin.Types;

namespace Flasky
{
    public class Route
    {
        readonly string _path;
        readonly string _method;
        readonly Func<OwinRequest, bool> _comparison;

        public Route(string path)
        {
            _path = path;
            Specificity = RouteSpecificity.Path;
            _comparison = (request) => { return request.Path == _path; };
        }

        public Route(string path, string method)
        {
            _path = path;
            _method = method;
            Specificity = RouteSpecificity.PathAndMethod;
            _comparison = (request) =>
                                    {
                                        return request.Path == _path && _method.Equals(request.Method, StringComparison.OrdinalIgnoreCase);
                                    };
        }

        public int Specificity { get; private set; }

        public bool HasMatch(OwinRequest request)
        {
            return _comparison(request);
        }
    }
}