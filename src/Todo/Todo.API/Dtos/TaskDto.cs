namespace Todo.API.Dtos
{
    public record CreateTaskDto(string Name, string Description);
    public record UpdateTaskDto(int Id, string Name, string Description, TasksStatus Status);

    public class TaskUserDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TasksStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
    }
}
