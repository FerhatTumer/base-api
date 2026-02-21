using FluentAssertions;
using TaskManagement.Application.Projects.Commands.CreateProject;
using Xunit;

namespace TaskManagement.Application.UnitTests;

public sealed class CreateProjectCommandValidatorTests
{
    private readonly CreateProjectCommandValidator _validator = new();

    [Fact]
    public async Task Validate_WhenNameIsEmpty_ReturnsValidationError()
    {
        CreateProjectCommand command = new(string.Empty, "description", 1);

        var result = await _validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task Validate_WhenOwnerIdIsNotPositive_ReturnsValidationError()
    {
        CreateProjectCommand command = new("Project", "description", 0);

        var result = await _validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "OwnerId");
    }
}
