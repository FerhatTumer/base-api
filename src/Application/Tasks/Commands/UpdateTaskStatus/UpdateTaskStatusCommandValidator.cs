using FluentValidation;

namespace TaskManagement.Application.Tasks.Commands.UpdateTaskStatus;

public sealed class UpdateTaskStatusCommandValidator : AbstractValidator<UpdateTaskStatusCommand>
{
    public UpdateTaskStatusCommandValidator()
    {
        RuleFor(x => x.ProjectId).GreaterThan(0).WithMessage("Project ID must be greater than 0.");
        RuleFor(x => x.TaskId).GreaterThan(0).WithMessage("Task ID must be greater than 0.");
        RuleFor(x => x.Status).IsInEnum().WithMessage("Task status is invalid.");
    }
}