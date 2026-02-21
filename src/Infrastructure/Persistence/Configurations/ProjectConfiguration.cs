using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Aggregates.ProjectAggregate;

namespace TaskManagement.Infrastructure.Persistence.Configurations;

public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(x => x.OwnerId)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
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

        builder.HasMany(x => x.TaskItems)
            .WithOne(x => x.Project)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_TaskItems_Projects_ProjectId");

        builder.HasIndex(x => x.OwnerId)
            .HasDatabaseName("IX_Projects_OwnerId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_Projects_Status");

        builder.HasIndex(x => x.IsDeleted)
            .HasDatabaseName("IX_Projects_IsDeleted");

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_Projects_Name");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}