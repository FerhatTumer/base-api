using FluentValidation;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Tasks.Commands.CreateTask;

public sealed class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .GreaterThan(0).WithMessage("Project ID must be greater than 0.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Task title is required.")
            .MaximumLength(200).WithMessage("Task title must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Priority value is invalid.");

        RuleFor(x => x.DueDate)
            .Must(x => !x.HasValue || x.Value > DateTimeOffset.UtcNow)
            .WithMessage("Due date must be in the future when set.");

        RuleFor(x => x.AssigneeId)
            .Must(x => !x.HasValue || x.Value > 0)
            .WithMessage("Assignee ID must be greater than 0 when set.");

        RuleFor(x => x.EstimatedHours)
            .Must(x => !x.HasValue || x.Value > 0)
            .WithMessage("Estimated hours must be positive when set.");
    }
}