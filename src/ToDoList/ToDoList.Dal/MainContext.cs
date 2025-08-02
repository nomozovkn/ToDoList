using Microsoft.EntityFrameworkCore;
using ToDoList.Dal.Configurations;
using ToDoList.Dal.Entity;

namespace ToDoList.Dal;

public class MainContext : DbContext
{
    public DbSet<ToDoItem> ToDoItems { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public MainContext(DbContextOptions<MainContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ToDoItemConfigurations());
        modelBuilder.ApplyConfiguration(new UserConfigurations());
    }
}
