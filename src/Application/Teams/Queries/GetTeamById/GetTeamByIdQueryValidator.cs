using FluentValidation;

namespace TaskManagement.Application.Teams.Queries.GetTeamById;

public sealed class GetTeamByIdQueryValidator : AbstractValidator<GetTeamByIdQuery>
{
    public GetTeamByIdQueryValidator()
    {
        RuleFor(x => x.TeamId)
            .GreaterThan(0)
            .WithMessage("Team ID must be greater than 0.");
    }
}