using Microsoft.EntityFrameworkCore;

namespace ToDoListWithDB;

public class AppDbContext : DbContext
{
    public DbSet<TaskItem> Tasks  { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        string dbPath = Path.Combine(projectDir, "tasks.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
}