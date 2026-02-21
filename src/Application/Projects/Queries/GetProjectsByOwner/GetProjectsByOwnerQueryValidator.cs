using FluentValidation;

namespace TaskManagement.Application.Projects.Queries.GetProjectsByOwner;

public sealed class GetProjectsByOwnerQueryValidator : AbstractValidator<GetProjectsByOwnerQuery>
{
    public GetProjectsByOwnerQueryValidator()
    {
        RuleFor(x => x.OwnerId)
            .GreaterThan(0)
            .WithMessage("Owner ID must be greater than 0.");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");
    }
}