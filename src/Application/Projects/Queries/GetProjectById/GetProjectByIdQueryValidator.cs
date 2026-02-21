using FluentValidation;

namespace TaskManagement.Application.Projects.Queries.GetProjectById;

public sealed class GetProjectByIdQueryValidator : AbstractValidator<GetProjectByIdQuery>
{
    public GetProjectByIdQueryValidator()
    {
        RuleFor(x => x.ProjectId)
            .GreaterThan(0)
            .WithMessage("Project ID must be greater than 0.");
    }
}