using FluentValidation;

namespace TaskManagement.Application.Teams.Commands.CreateTeam;

public sealed class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Team name is required.")
            .MaximumLength(100).WithMessage("Team name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.LeaderId)
            .GreaterThan(0).WithMessage("Leader ID must be greater than 0.");
    }
}