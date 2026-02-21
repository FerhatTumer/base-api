using FluentValidation;

namespace TaskManagement.Application.Teams.Commands.AddTeamMember;

public sealed class AddTeamMemberCommandValidator : AbstractValidator<AddTeamMemberCommand>
{
    public AddTeamMemberCommandValidator()
    {
        RuleFor(x => x.TeamId).GreaterThan(0).WithMessage("Team ID must be greater than 0.");
        RuleFor(x => x.UserId).GreaterThan(0).WithMessage("User ID must be greater than 0.");
        RuleFor(x => x.Role).IsInEnum().WithMessage("Team role is invalid.");
    }
}