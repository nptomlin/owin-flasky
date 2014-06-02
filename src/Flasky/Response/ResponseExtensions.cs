using ServiceStack;

namespace Flasky.Response
{
    public static class ResponseExtensions
    {
        public static object AsJson<T>(this T source)
        {
            return source.ToJson();
        }

        public static IResponse AsJsonResponse<T>(this T source) where T : class
        {
            return JsonResponse<T>.Create(source);
        }
    }
}
