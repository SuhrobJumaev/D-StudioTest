namespace Todo.API.Dtos
{
    public record CreateUserDto(string FullName, string Email, string Password,UserRole Role);
    public record UpdateUserDto(int Id, string FullName, string Email, string? Password, UserRole Role);
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
