using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Aggregates.TeamAggregate;

namespace TaskManagement.Infrastructure.Persistence.Configurations;

public sealed class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.ToTable("TeamMembers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Role)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.JoinedAt)
            .IsRequired();

        builder.Property(x => x.TeamId)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property<uint>("RowVersion")
            .HasColumnName("xmin")
            .ValueGeneratedOnAddOrUpdate()
            .IsConcurrencyToken();

        builder.HasOne(x => x.Team)
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.TeamId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_TeamMembers_Teams_TeamId");

        builder.HasIndex(x => x.TeamId)
            .HasDatabaseName("IX_TeamMembers_TeamId");

        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_TeamMembers_UserId");

        builder.HasIndex(x => new { x.TeamId, x.UserId })
            .IsUnique()
            .HasDatabaseName("IX_TeamMembers_TeamId_UserId");

        builder.HasIndex(x => x.IsDeleted)
            .HasDatabaseName("IX_TeamMembers_IsDeleted");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}