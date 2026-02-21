using FluentValidation;

namespace TaskManagement.Application.Projects.Commands.ArchiveProject;

public sealed class ArchiveProjectCommandValidator : AbstractValidator<ArchiveProjectCommand>
{
    public ArchiveProjectCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .GreaterThan(0)
            .WithMessage("Project ID must be greater than 0.");
    }
}