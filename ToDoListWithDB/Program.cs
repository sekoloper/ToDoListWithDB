namespace ToDoListWithDB;

class Program
{
    static async Task Main(string[] args)
    {
        using var context = new AppDbContext();
        var taskService =  new TaskService(context);
        int actionIndex;
        
    }
}