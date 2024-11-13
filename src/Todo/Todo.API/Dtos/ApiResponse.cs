using System.Net;

namespace Todo.API.Dtos
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; init; }
        public string Message { get; init; }
        public required IEnumerable<ValidationResponse> Errors { get; init; }
    }

    public class ValidationResponse
    {
        public required string PropertyName { get; init; }
        public required string Message { get; init; }
    }
}
