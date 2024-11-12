using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Todo.API.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Salt)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.Role)
                .IsRequired();

            builder.Property( u => u.CreatedDate)
                .HasDefaultValueSql("GETDATE()");


            builder.HasIndex(e => e.Email)
                .IsUnique();

        }
    }
}
