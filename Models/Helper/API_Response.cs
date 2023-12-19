using System.Net;

namespace Models.Helper
{
    public class API_Response
    {
        public API_Response()
        {
            
        }

        public bool IsSuccess { get; set; }
        public object? Result { get; set; }
        public string? ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public static API_Response Success(object? result)
        {
            return new API_Response()
            {
                IsSuccess = true,
                Result = result,
                StatusCode = HttpStatusCode.OK,
                ErrorMessage = null
            };
        }

        public static API_Response Failure(string exception, HttpStatusCode statusCode)
        {
            return new API_Response()
            {
                IsSuccess = false,
                ErrorMessage = exception,
                StatusCode = statusCode,
                Result = null
            };
        }
    }
}
