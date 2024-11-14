
using System.Text.Json.Serialization;

namespace Todo.API.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [JsonIgnore]
        public string Password { get; set; } = string.Empty;
        [JsonIgnore]
        public string Salt { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<TaskModel> Tasks { get; set; }
    }
}
