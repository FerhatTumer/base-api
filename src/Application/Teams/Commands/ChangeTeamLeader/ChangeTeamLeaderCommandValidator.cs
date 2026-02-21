using FluentValidation;

namespace TaskManagement.Application.Teams.Commands.ChangeTeamLeader;

public sealed class ChangeTeamLeaderCommandValidator : AbstractValidator<ChangeTeamLeaderCommand>
{
    public ChangeTeamLeaderCommandValidator()
    {
        RuleFor(x => x.TeamId).GreaterThan(0).WithMessage("Team ID must be greater than 0.");
        RuleFor(x => x.NewLeaderId).GreaterThan(0).WithMessage("New leader ID must be greater than 0.");
    }
}