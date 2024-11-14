using Todo.API.Dtos;

namespace Todo.API.Validators
{
    public class UserCreateValidator : AbstractValidator<CreateUserDto>
    {
        private const string emailPattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
        public UserCreateValidator()
        {
            RuleFor(u => u.FullName).NotEmpty();
            RuleFor(u => u.Email).Matches(emailPattern).NotEmpty();
            RuleFor(u => u.Password).MinimumLength(5).NotEmpty();
            RuleFor(u => u.Role).IsInEnum();
        }
    }
}
