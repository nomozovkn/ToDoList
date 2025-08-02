using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ToDoList.Dal.Entity;

namespace ToDoList.Dal.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.UserId);

        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.LastName).IsRequired(false).HasMaxLength(50);

        builder.Property(u => u.Email).IsRequired().HasMaxLength(320);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(15);
        builder.HasIndex(u => u.PhoneNumber).IsUnique();

        builder.Property(u => u.UserName).IsRequired().HasMaxLength(50);
        builder.HasIndex(u => u.UserName).IsUnique();

        builder.Property(u => u.Salt).IsRequired().HasMaxLength(36);

        builder.Property(u => u.Password).IsRequired().HasMaxLength(128);

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rf => rf.UserId);

        builder.HasMany(u => u.ToDoItems)
            .WithOne(i => i.User)
            .HasForeignKey(i => i.UserId);
    }
}
