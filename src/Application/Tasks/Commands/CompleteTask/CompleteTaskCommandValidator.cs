using FluentValidation;

namespace TaskManagement.Application.Tasks.Commands.CompleteTask;

public sealed class CompleteTaskCommandValidator : AbstractValidator<CompleteTaskCommand>
{
    public CompleteTaskCommandValidator()
    {
        RuleFor(x => x.ProjectId).GreaterThan(0).WithMessage("Project ID must be greater than 0.");
        RuleFor(x => x.TaskId).GreaterThan(0).WithMessage("Task ID must be greater than 0.");
    }
}