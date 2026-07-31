namespace ToDoListWithDB;

class Program
{
    static async Task Main(string[] args)
    {
        using var context = new AppDbContext();
        var taskService =  new TaskService(context);
        int actionIndex;
        Console.WriteLine("Welcome to ToDoList.");
        do
        {
            Console.WriteLine("1 - Add new task");
            Console.WriteLine("2 - Show all tasks");
            Console.WriteLine("3 - Complete task");
            Console.WriteLine("4 - Delete task");
            Console.WriteLine("5 - Show pending task");
            Console.WriteLine("6 - Add new category");
            Console.WriteLine("0 - Exit");
            Console.WriteLine("Enter the action index:");
            if (int.TryParse(Console.ReadLine(), out actionIndex))
            {
                switch (actionIndex)
                {
                    case 1: await AddNewTaskAsync(taskService); break;
                    case 2: await ShowAllTasksAsync(taskService); break;
                    case 3: await CompleteTaskAsync(taskService); break;
                    case 4: await DeleteTaskAsync(taskService); break;
                    case 5: await ShowPendingTasksAsync(taskService); break;
                    case 6: await AddNewCategoryAsync(taskService); break;
                    case 0: break;
                    default: Console.WriteLine("Invalid input"); break;
                }
            }
            else
                Console.WriteLine("Invalid input");
        } while (actionIndex != 0);
        
        Console.WriteLine("Goodbye!");
    }
    
    static async Task AddNewTaskAsync(TaskService taskService)
    {
        Console.WriteLine("Enter the title of the task:");
        string? title = Console.ReadLine();
        
        if (string.IsNullOrEmpty(title))
        {
            Console.WriteLine("Title cannot be empty");
            return;
        }
        
        Console.WriteLine("Available categories:");
        foreach (var category in await taskService.GetAllCategoriesAsync())
            Console.WriteLine($"{category.Id}: {category.Name}");
        
        Console.WriteLine("Enter the id of category of the task:");
        if (int.TryParse(Console.ReadLine(), out int categoryId))
        {
            if (await taskService.CategoryExistsAsync(categoryId))
                await taskService.AddTaskAsync(title, categoryId);
            else
                Console.WriteLine($"The category with id {categoryId} not found");
        }
        else 
            Console.WriteLine("Invalid input");
    }

    static async Task ShowAllTasksAsync(TaskService taskService)
    {
        foreach (var task in await taskService.GetAllTasksAsync())
        {
            if (task.IsDone)
                Console.WriteLine($"{task.Category.Name} | {task.Id}: {task.Title} [completed]");
            else
            {
                Console.WriteLine($"{task.Category.Name} | {task.Id}: {task.Title}");
            }
        }
    }

    static async Task CompleteTaskAsync(TaskService taskService)
    {
        Console.WriteLine("Enter the id of the task you want to complete:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                await taskService.CompleteTaskAsync(id);
                Console.WriteLine($"Task {id} is completed.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
            Console.WriteLine("Invalid input.");
    }

    static async Task DeleteTaskAsync(TaskService taskService)
    {
        Console.WriteLine("Enter the id of the task you want to delete:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                await taskService.DeleteTaskAsync(id);
                Console.WriteLine($"Task {id} is deleted.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
            Console.WriteLine("Invalid input.");
    }

    static async Task ShowPendingTasksAsync(TaskService taskService)
    {
        foreach (var task in await taskService.GetPendingTasksAsync())
            Console.WriteLine($"{task.Category.Name} | {task.Id}: {task.Title}");
    }

    static async Task AddNewCategoryAsync(TaskService taskService)
    {
        Console.WriteLine("Enter the category name:");
        string? categoryName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(categoryName))
        {
            Console.WriteLine("Category name cannot be empty");
            return;
        }
        await taskService.AddCategoryAsync(categoryName);
    }
}