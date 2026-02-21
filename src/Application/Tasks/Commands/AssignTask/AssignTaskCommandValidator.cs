using FluentValidation;

namespace TaskManagement.Application.Tasks.Commands.AssignTask;

public sealed class AssignTaskCommandValidator : AbstractValidator<AssignTaskCommand>
{
    public AssignTaskCommandValidator()
    {
        RuleFor(x => x.ProjectId).GreaterThan(0).WithMessage("Project ID must be greater than 0.");
        RuleFor(x => x.TaskId).GreaterThan(0).WithMessage("Task ID must be greater than 0.");
        RuleFor(x => x.AssigneeId).GreaterThan(0).WithMessage("Assignee ID must be greater than 0.");
    }
}