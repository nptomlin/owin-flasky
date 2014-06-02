using System;
using System.Threading.Tasks;
using Owin.Types;
using ServiceStack;

namespace Flasky.Response
{
    public class JsonResponse<T> : IResponse where T : class
    {
        private readonly T _objectToWrite;

        public JsonResponse(T objectToWrite)
        {
            _objectToWrite = objectToWrite;
        }

        public Task Write(OwinResponse response)
        {
            response.ContentType = "application/json";
            return response.WriteAsync(_objectToWrite.ToJson());
        }

        public static JsonResponse<T> Create(T objectToWrite)
        {
            return new JsonResponse<T>(objectToWrite);
        }
    }
}