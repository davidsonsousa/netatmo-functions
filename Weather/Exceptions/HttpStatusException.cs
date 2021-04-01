using System;
using System.Net;
using System.Net.Http;

namespace Weather.Exceptions
{
    public class HttpStatusException : HttpRequestException
    {
        public int StatusCode { get; private set; }

        public HttpStatusException(HttpStatusCode statusCode)
        {
            StatusCode = (int)statusCode;
        }

        public HttpStatusException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = (int)statusCode;
        }

        public HttpStatusException(HttpStatusCode statusCode, string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = (int)statusCode;
        }
    }
}
