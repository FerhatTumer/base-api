using FluentValidation;

namespace TaskManagement.Application.Tasks.Queries.GetTasksByProject;

public sealed class GetTasksByProjectQueryValidator : AbstractValidator<GetTasksByProjectQuery>
{
    public GetTasksByProjectQueryValidator()
    {
        RuleFor(x => x.ProjectId).GreaterThan(0).WithMessage("Project ID must be greater than 0.");
        RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page number must be greater than 0.");
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}