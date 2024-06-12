using TTP_2_4.Models;
using Task = TTP_2_4.Models.Task;

public static class Requests
{
    private static Db1Context db = new Db1Context();

    public static bool CheckDBAvailability()
    {
        bool isDBAvalaible = db.Database.CanConnect();
        return isDBAvalaible;
    }

    public static int UserАuthorization(string username, string password)
    {
        User user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

        if (user != null)
        {
            return user.Userid;
        }
        
        return -1;
    }
    
    public static int UserRegistration(string username, string password)
    {
        bool usernameCheck = db.Users.Any(u => u.Username == username);
        
        if (!usernameCheck)
        {
            User newUser = new User()
            {
                Username = username,
                Password = password
            };

            db.Users.Add(newUser);
            db.SaveChanges();

            return newUser.Userid;
        }
        
        return -1;
    }

    public static List<Task> GetAllTasks(int userId)
    {
        List<Task> tasks = db.Tasks.Where(t => t.Userid == userId).ToList();
        return tasks;
    }
    
    public static Task GetTask(int taskId)
    {
        Task task = db.Tasks.FirstOrDefault(t => t.Taskid == taskId);

        return task;
    }

    public static void EditTask(int taskId, string title, string description, DateTime duedate)
    {
        Task task = db.Tasks.FirstOrDefault(t => t.Taskid == taskId);

        task.Title = title;
        task.Description = description;
        task.Duedate = duedate;
    }
    
    public static int NewTask(int userId, string title, string description, DateTime duedate)
    {
        bool taskCheck = db.Tasks.Any(t => t.Title == title && t.Description == description && t.Duedate == duedate && t.Userid == userId);

        if (!taskCheck)
        {
            Task newTask = new Task()
            {
                Title = title,
                Description = description,
                Duedate = duedate,
                Userid = userId
            };

            db.Tasks.Add(newTask);
            db.SaveChanges();
            
            return newTask.Taskid;
        }

        return -1;
    }

    public static void DeleteTask(int taskId)
    {
        Task task = db.Tasks.FirstOrDefault(t => t.Taskid == taskId);

        if (task != null)
        {
            db.Tasks.Remove(task);
            db.SaveChanges();
        }
    }
    
    public static int TaskCheck(int userId, int taskId)
    {
        bool taskCheck = db.Tasks.Any(t => t.Taskid == taskId && t.Userid == userId);
        
        if (!taskCheck)
        {
            return -1;
        }

        return 0;
    }
    
    public static void PrintTask(Task task)
    {
        Console.WriteLine($"\nID задачи: {task.Taskid}");
        Console.WriteLine($"Название: {task.Title}");
        Console.WriteLine($"Описание: {task.Description}");
        Console.WriteLine($"Сроки: {task.Duedate}");
    }
    
    public static void PrintTitleAndIdTask(Task task)
    {
        Console.WriteLine($"\nID задачи: {task.Taskid} | Название: {task.Title}");
    }
}