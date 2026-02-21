using FluentValidation;

namespace TaskManagement.Application.Tasks.Queries.GetTaskById;

public sealed class GetTaskByIdQueryValidator : AbstractValidator<GetTaskByIdQuery>
{
    public GetTaskByIdQueryValidator()
    {
        RuleFor(x => x.TaskId)
            .GreaterThan(0)
            .WithMessage("Task ID must be greater than 0.");
    }
}