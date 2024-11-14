namespace Todo.API.Validators
{
    public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
    {
        
            public CreateTaskValidator()
            {
                RuleFor(t => t.Name).NotEmpty();
                RuleFor(t => t.Description).NotEmpty();
            }
        
    }
}
