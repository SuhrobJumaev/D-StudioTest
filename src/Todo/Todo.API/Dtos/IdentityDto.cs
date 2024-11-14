namespace Todo.API.Dtos
{
    public record LoginDto(string Email, string Password);
    public record RefreshTokenDto (int UserId, string RefreshToken);

}
