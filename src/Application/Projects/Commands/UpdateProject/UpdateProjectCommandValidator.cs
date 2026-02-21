using FluentValidation;

namespace TaskManagement.Application.Projects.Commands.UpdateProject;

public sealed class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .GreaterThan(0)
            .WithMessage("Project ID must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Project name is required.")
            .MaximumLength(200)
            .WithMessage("Project name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("Description must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}