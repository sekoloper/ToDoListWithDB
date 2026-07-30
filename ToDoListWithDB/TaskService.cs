using Microsoft.EntityFrameworkCore;

namespace ToDoListWithDB;

public class TaskService
{
    private readonly AppDbContext _context;
    
    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem> AddTaskAsync(string title, int categoryId)
    {
        var task = new TaskItem
        {
                        Title = title,
                        IsDone = false,
                        CreatedAt = DateTime.Now,
                        CategoryId = categoryId
        };
        
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task DeleteTaskAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task is null)
            throw new ArgumentException($"Task with id {id} not found");
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }
    
    public async Task CompleteTaskAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task is null)
            throw new ArgumentException($"Task with id {id} not found");
        task.IsDone = true;
        await _context.SaveChangesAsync();
    }

    public async Task<List<TaskItem>> GetAllTasksAsync()
    {
        return await _context.Tasks.ToListAsync();
    }
    
    public async Task<List<TaskItem>> GetPendingTaskAsync()
    {
        return await _context.Tasks.Where(t => !t.IsDone).ToListAsync();
    }

    public async Task<List<TaskItem>> GetTasksByCategoryAsync(int categoryId)
    {
        return await _context.Tasks.Where(t => t.CategoryId == categoryId).ToListAsync();
    }
    
    public async Task<Category> AddCategoryAsync(string name)
    {
        var category = new Category { Name = name };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }
    
    public async Task<bool> CategoryExistsAsync(int categoryId)
    {
        return await _context.Categories.AnyAsync(c => c.Id == categoryId);
    }
}