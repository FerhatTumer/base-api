using FluentAssertions;
using FluentValidation;
using MediatR;
using TaskManagement.Application.Common.Behaviors;
using TaskManagement.Application.Common.Exceptions;
using Xunit;

namespace TaskManagement.Application.UnitTests;

public sealed class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_WhenValidationFails_ThrowsValidationException()
    {
        ValidationBehavior<SampleRequest, SampleResponse> behavior = new(new[] { new SampleRequestValidator() });
        SampleRequest request = new(string.Empty);

        Func<Task> act = async () => await behavior.Handle(request, _ => Task.FromResult(new SampleResponse("ok")), CancellationToken.None);

        await act.Should().ThrowAsync<TaskManagement.Application.Common.Exceptions.ValidationException>();
    }

    [Fact]
    public async Task Handle_WhenValidationSucceeds_InvokesNext()
    {
        ValidationBehavior<SampleRequest, SampleResponse> behavior = new(new[] { new SampleRequestValidator() });
        SampleRequest request = new("valid");

        SampleResponse response = await behavior.Handle(request, _ => Task.FromResult(new SampleResponse("done")), CancellationToken.None);

        response.Value.Should().Be("done");
    }

    private sealed record SampleRequest(string Name) : IRequest<SampleResponse>;

    private sealed record SampleResponse(string Value);

    private sealed class SampleRequestValidator : AbstractValidator<SampleRequest>
    {
        public SampleRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
