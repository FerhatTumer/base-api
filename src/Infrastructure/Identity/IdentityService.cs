namespace TaskManagement.Infrastructure.Identity;

public sealed class IdentityService
{
    public Task<bool> UserExistsAsync(int userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(userId > 0);
    }
}