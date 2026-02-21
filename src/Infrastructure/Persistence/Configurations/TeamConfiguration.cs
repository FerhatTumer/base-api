using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Aggregates.TeamAggregate;

namespace TaskManagement.Infrastructure.Persistence.Configurations;

public sealed class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(x => x.LeaderId)
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

        builder.HasMany(x => x.Members)
            .WithOne(x => x.Team)
            .HasForeignKey(x => x.TeamId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_TeamMembers_Teams_TeamId");

        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("IX_Teams_Name");

        builder.HasIndex(x => x.LeaderId)
            .HasDatabaseName("IX_Teams_LeaderId");

        builder.HasIndex(x => x.IsDeleted)
            .HasDatabaseName("IX_Teams_IsDeleted");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}