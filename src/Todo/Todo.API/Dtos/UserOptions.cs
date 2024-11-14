using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Todo.API.Dtos
{
    public class UserQueryOptions
    {
        public int Page { get; set; } = 1;
        public string? Search { get; set; }

        [BindNever]
        public int? Limit { get; set; }

        [BindNever]
        public int? Skip { get; set; }
        [BindNever]
        public int? ItemsOfPage { get; set; } = 20;
    }
}
