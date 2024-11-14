using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Todo.API.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenModel>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenModel> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.UserId)
                .IsRequired();

            builder.Property(r => r.Token)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(r => r.ExpiryDate)
                .IsRequired();

            builder.Property(r => r.IsRevoked)
                .HasDefaultValue(false);

            builder.HasIndex(r => new { r.UserId, r.IsRevoked })
               .HasDatabaseName("RefreshToken_UserId_IsRevoked");

            builder.HasIndex(r => new { r.UserId, r.Token })
               .HasDatabaseName("RefreshToken_UserId_Token");
        }
    }
}
