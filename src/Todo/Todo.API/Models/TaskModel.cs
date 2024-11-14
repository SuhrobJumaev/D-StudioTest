

namespace Todo.API.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TasksStatus Status { get; set; } = TasksStatus.Pendig;
        public DateTime CreatedDate { get; set; } 
        public DateTime? ModifiedDate { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
    }
}
