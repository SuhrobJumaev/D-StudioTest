using System.Net;

namespace Todo.API.Dtos
{
    public class ApiResponse
    {
        public ApiErrorCode Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<Param> Params { get; set; } = new();
        public  IEnumerable<ValidationResponse> Errors { get; set; }
    }

    public class ValidationResponse
    {
        public required string PropertyName { get; init; }
        public required string Message { get; init; }
    }

    public class Param
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
