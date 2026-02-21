using FluentValidation;

namespace TaskManagement.Application.Teams.Queries.GetTeamsByLeader;

public sealed class GetTeamsByLeaderQueryValidator : AbstractValidator<GetTeamsByLeaderQuery>
{
    public GetTeamsByLeaderQueryValidator()
    {
        RuleFor(x => x.LeaderId).GreaterThan(0).WithMessage("Leader ID must be greater than 0.");
        RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page number must be greater than 0.");
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}