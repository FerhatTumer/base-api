using FluentValidation;

namespace TaskManagement.Application.Teams.Commands.RemoveTeamMember;

public sealed class RemoveTeamMemberCommandValidator : AbstractValidator<RemoveTeamMemberCommand>
{
    public RemoveTeamMemberCommandValidator()
    {
        RuleFor(x => x.TeamId).GreaterThan(0).WithMessage("Team ID must be greater than 0.");
        RuleFor(x => x.UserId).GreaterThan(0).WithMessage("User ID must be greater than 0.");
        RuleFor(x => x.NewLeaderId)
            .Must(x => !x.HasValue || x.Value > 0)
            .WithMessage("New leader ID must be greater than 0 when provided.");
    }
}