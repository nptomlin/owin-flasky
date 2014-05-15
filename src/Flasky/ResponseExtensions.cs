using ServiceStack;

namespace Flasky
{
    public static class ResponseExtensions
    {
        public static object AsJson<T>(this T source)
        {
            return source.ToJson();
        }
    }
}
