using Harmony.Persistence.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YunenApi.Persistence.Context.Configurations;

public class UserEntityConfigurations : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.UserName);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Phone);

        builder.Property(e => e.Hash)
            .IsRequired();

        builder.Property(e => e.Salt);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.HasAlternateKey(e => e.UserName);
        builder.HasAlternateKey(e => e.Email);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(200);

    }


}