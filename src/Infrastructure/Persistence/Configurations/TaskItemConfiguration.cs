using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Aggregates.ProjectAggregate;

namespace TaskManagement.Infrastructure.Persistence.Configurations;

public sealed class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("TaskItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(x => x.Priority)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.DueDate)
            .IsRequired(false);

        builder.Property(x => x.AssigneeId)
            .IsRequired(false);

        builder.Property(x => x.ProjectId)
            .IsRequired();

        builder.Property(x => x.EstimatedHours)
            .HasPrecision(5, 2)
            .IsRequired(false);

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

        builder.HasOne(x => x.Project)
            .WithMany(x => x.TaskItems)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_TaskItems_Projects_ProjectId");

        builder.HasIndex(x => x.ProjectId)
            .HasDatabaseName("IX_TaskItems_ProjectId");

        builder.HasIndex(x => x.AssigneeId)
            .HasDatabaseName("IX_TaskItems_AssigneeId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_TaskItems_Status");

        builder.HasIndex(x => x.DueDate)
            .HasDatabaseName("IX_TaskItems_DueDate");

        builder.HasIndex(x => x.IsDeleted)
            .HasDatabaseName("IX_TaskItems_IsDeleted");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}