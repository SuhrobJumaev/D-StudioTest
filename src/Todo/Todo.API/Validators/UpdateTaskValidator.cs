namespace Todo.API.Validators
{
    public class UpdateTaskValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskValidator() 
        {
            RuleFor(t => t.Id).GreaterThan(0);
            RuleFor(t => t.Name).NotEmpty();
            RuleFor(t => t.Description).NotEmpty();
            RuleFor(t => t.Status).IsInEnum();
        }
    }
}
