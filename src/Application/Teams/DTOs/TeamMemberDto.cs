using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Teams.DTOs;

public record TeamMemberDto
{
    public int Id { get; init; }

    public int UserId { get; init; }

    public TeamRole Role { get; init; }

    public DateTimeOffset JoinedAt { get; init; }
}