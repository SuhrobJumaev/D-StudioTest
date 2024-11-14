namespace Todo.API.Dtos
{
    public class PaginationResponse<T>
    {
        public int CurrentPage { get; set; }
        public int CountPage { get; set; }
        public IEnumerable<T> Enttities { get; set; }
    }
}
