using FluentValidation;

namespace TaskManagement.Application.Tasks.Queries.GetTasksByAssignee;

public sealed class GetTasksByAssigneeQueryValidator : AbstractValidator<GetTasksByAssigneeQuery>
{
    public GetTasksByAssigneeQueryValidator()
    {
        RuleFor(x => x.AssigneeId).GreaterThan(0).WithMessage("Assignee ID must be greater than 0.");
        RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page number must be greater than 0.");
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}