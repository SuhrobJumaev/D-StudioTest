using Todo.API.Dtos;

namespace Todo.API.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        private const string emailPattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

        public UpdateUserValidator()
        {
            RuleFor(u => u.Id).GreaterThan(0);
            RuleFor(u => u.FullName).NotEmpty();
            RuleFor(u => u.Password).MinimumLength(5).NotEmpty().When(u => u.Password is not null);
            RuleFor(u => u.Email).Matches(emailPattern).NotEmpty();
            RuleFor(u => u.Role).IsInEnum();
        }
    }
}
