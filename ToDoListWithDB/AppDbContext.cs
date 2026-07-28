using Microsoft.EntityFrameworkCore;

namespace ToDoListWithDB;

public class AppDbContext : DbContext
{
    public DbSet<TaskItem> Tasks  { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=tasks.db");
    }
}