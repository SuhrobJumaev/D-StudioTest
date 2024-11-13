using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Todo.API.Data.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<TaskModel>
    {
        public void Configure(EntityTypeBuilder<TaskModel> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Description)
                .HasMaxLength(500);

            builder.Property(t => t.Status)
                .HasDefaultValue(TasksStatus.Pendig)
                 .HasConversion<short>();

            builder.Property(t => t.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(t => t.ModifiedDate)
                .IsRequired(false);
        }
    }

}
