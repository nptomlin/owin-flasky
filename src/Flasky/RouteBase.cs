using System;
using Owin.Types;

namespace Flasky
{
    public abstract class RouteBase
    {
        protected string Path;
        readonly string _method;
        readonly RouteSetting _setting;

        public int Specificity { get; private set; }

        protected RouteBase(string path)
        {
            Path = path;
            Specificity = RouteSpecificity.Path;
            _setting = RouteSetting.Path;
        }

        protected RouteBase(string path, string method)
        {
            Path = path;
            _method = method;
            Specificity = RouteSpecificity.PathAndMethod;
            _setting = RouteSetting.PathAndMethod;
        }

        public bool HasMatch(OwinRequest request)
        {
            switch (_setting)
            {
                case RouteSetting.Path :
                    return HasMatch(request.Path);
                case RouteSetting.PathAndMethod:
                    return HasMatch(request.Path) && _method.Equals(request.Method, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public abstract bool HasMatch(string path);
    }
}