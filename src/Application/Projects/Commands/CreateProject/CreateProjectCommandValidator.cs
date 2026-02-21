using FluentValidation;

namespace TaskManagement.Application.Projects.Commands.CreateProject;

public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        const string NameRequired = "Project name is required.";
        const string NameMax = "Project name must not exceed 200 characters.";
        const string DescriptionMax = "Description must not exceed 2000 characters.";
        const string OwnerInvalid = "Owner ID must be greater than 0.";

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(NameRequired)
            .MaximumLength(200).WithMessage(NameMax);

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage(DescriptionMax)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.OwnerId)
            .GreaterThan(0).WithMessage(OwnerInvalid);
    }
}