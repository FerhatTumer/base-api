namespace TaskManagement.Infrastructure.Identity;

public sealed class ApplicationUser
{
    public int Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}