internal class Program
{
    private static void Main(string[] args)
    {
        bool isDBAbailable = Requests.CheckDBAvailability();
        if (!isDBAbailable)
        {
            Console.WriteLine("База данных, в данный момент недоступна :(");
        }
        else
        {
            int userId = 0;
            bool AuthMenuWork = true;
            while (AuthMenuWork)
            {
                int AuthMenu =
                    StringToInt(
                        AntiEmptyStringMenu(
                            "1 - Авторизация \n2 - Регистрация \n3 - Выход \n\nВыберите нужное действие: "));
                switch (AuthMenu)
                {
                    case 0:
                        break;
                    case 1:
                        userId = LoginUser();
                        AuthMenuWork = false;
                        break;
                    case 2:
                        userId = RegisterUser();
                        AuthMenuWork = false;
                        break;
                    case 3:
                        AuthMenuWork = false;
                        break;
                    default:
                        Console.WriteLine("| Неизвестное действие! |");
                        break;
                }
            }

            if (userId != 0) MainMenu(userId);
        }
    }

    private static int LoginUser()
    {
        int userId = 0;
        string username = AntiEmptyString("Введите имя пользователя (string): ");
        string password = AntiEmptyString("Введите пароль (string): ");

        userId = Requests.UserАuthorization(username, password);

        if (userId == -1)
        {
            Console.WriteLine("\n| Ошибка: Неверный логин или пароль! |");
            LoginUser();
        }
        
        return userId;
    }

    private static int RegisterUser()
    {
        int userId = 0;
        string username = AntiEmptyString("Придумайте имя пользователя (string): ");

        while (true)
        {
            string password = AntiEmptyString("Придумайте надежный пароль (string): ");
            string passwordRepeat = AntiEmptyString("Повторно введите пароль (string): ");

            if (password != passwordRepeat)
            {
                Console.WriteLine("\n| Ошибка: Пароли несовпадают! |");
                continue;
            }

            userId = Requests.UserRegistration(username, password);
            
            if (userId == -1)
            {
                Console.WriteLine("\n| Ошибка: Пользователь с таким логином, уже существует! |");
                RegisterUser();
            }
            else break;
        }

        return userId;
    }

    private static void MainMenu(int userId)
    {
        Console.WriteLine("\n| Успешная авторизация! |");

        bool mainMenuWork = true;
        while (mainMenuWork)
        {
            int mainMenu =
                StringToInt(
                    AntiEmptyStringMenu(
                        "1 - Просмотреть задачи \n2 - Добавить новую задачу \n3 - Удалить задачу \n4 - Отредактировать задачу \n5 - Выход \n\nВыберите нужное действие: "));
            switch (mainMenu)
            {
                case 0:
                    break;
                case 1:
                    var allTasks = Requests.GetAllTasks(userId);

                    if (allTasks.Count == 0)
                    {
                        Console.WriteLine("\n| Задачи - отсутствуют! |");
                    }
                    else
                    {
                        bool viewMenuWork = true;
                        while (viewMenuWork)
                        {
                            int viewMenu =
                                StringToInt(
                                    AntiEmptyStringMenu(
                                        "1 - Просмотреть актуальные задачи \n2 - Вывести список всех задач, которые уже прошли \n3 - Вывести все задачи \n4 - Назад \n\nВыберите нужное действие: "));
                            switch (viewMenu)
                            {
                                case 0:
                                    break;
                                case 1:
                                    var presentTasks = allTasks.FindAll(task => DateTime.Now.Date <= task.Duedate.Date &&  (task.Duedate.Date > DateTime.Now.Date || task.Duedate.TimeOfDay > DateTime.Now.TimeOfDay));

                                    if (presentTasks.Count == 0)
                                    {
                                        Console.WriteLine("\n| Актуальные задачи - отсутствуют! |");
                                    }
                                    else
                                    {
                                        bool viewPresentMenuWork = true;
                                        while (viewPresentMenuWork)
                                        {
                                            int viewPresentMenu =
                                                StringToInt(
                                                    AntiEmptyStringMenu(
                                                        "1 - Вывести все задачи \n2 - Вывести задачи на сегодня \n3 - Вывести задачи на завтра \n4 - Вывести задачи на неделю \n5 - Назад \n\nВыберите нужное действие: "));
                                            switch (viewPresentMenu)
                                            {
                                                case 0:
                                                    break;
                                                case 1:
                                                    foreach (var task in presentTasks)
                                                    {
                                                        Requests.PrintTask(task);
                                                    }

                                                    break;
                                                case 2:
                                                    var todayTasks = presentTasks.FindAll(task =>
                                                        DateTime.Today == task.Duedate.Date &&
                                                        task.Duedate.TimeOfDay > DateTime.Now.TimeOfDay);

                                                    if (todayTasks.Count == 0)
                                                    {
                                                        Console.WriteLine("\n| Задачи на сегодня - отсутствуют! |");
                                                    }

                                                    foreach (var task in todayTasks)
                                                    {
                                                        Requests.PrintTask(task);
                                                    }

                                                    break;
                                                case 3:
                                                    var tomorrowTasks = presentTasks.FindAll(task =>
                                                        DateTime.Today.AddDays(1) == task.Duedate.Date);

                                                    if (tomorrowTasks.Count == 0)
                                                    {
                                                        Console.WriteLine("\n| Задачи на завтра - отсутствуют! |");
                                                    }

                                                    foreach (var task in tomorrowTasks)
                                                    {
                                                        Requests.PrintTask(task);
                                                    }

                                                    break;
                                                case 4:
                                                    var weekTasks = presentTasks.FindAll(task =>
                                                        DateTime.Today <= task.Duedate.Date &&
                                                        task.Duedate.Date <= DateTime.Today.AddDays(7));

                                                    if (weekTasks.Count == 0)
                                                    {
                                                        Console.WriteLine("\n| Задачи на завтра - отсутствуют! |");
                                                    }

                                                    foreach (var task in weekTasks)
                                                    {
                                                        Requests.PrintTask(task);
                                                    }

                                                    break;
                                                case 5:
                                                    viewPresentMenuWork = false;
                                                    break;
                                                default:
                                                    Console.WriteLine("\n| Неизвестное действие! |");
                                                    break;
                                            }
                                        }
                                    }

                                    break;
                                case 2:
                                    var pastTasks = allTasks.FindAll(task =>
                                        task.Duedate.Date < DateTime.Today ||
                                        (task.Duedate.Date == DateTime.Today &&
                                         task.Duedate.TimeOfDay < DateTime.Now.TimeOfDay));

                                    if (pastTasks.Count == 0)
                                    {
                                        Console.WriteLine("\n| Задачи, которые уже прошли - отсутствуют! |");
                                    }
                                    else
                                    {
                                        foreach (var task in pastTasks)
                                        {
                                            Requests.PrintTask(task);
                                        }
                                    }

                                    break;
                                case 3:
                                    foreach (var task in allTasks)
                                    {
                                        Requests.PrintTask(task);
                                    }

                                    break;
                                case 4:
                                    viewMenuWork = false;
                                    break;
                                default:
                                    Console.WriteLine("\n| Неизвестное действие! |");
                                    break;
                            }
                        }
                    }

                    break;
                case 2:
                    string title = AntiEmptyString("Введите название задачи (string): ");
                    string description = AntiEmptyString("Введите описание задачи (string): ");
                    while (true)
                    {
                        string dueDateString = AntiEmptyString("Введите дату и время (в формате ДД.ММ.ГГГГ ЧЧ:ММ): ");

                        if (DateTime.TryParseExact(dueDateString, "dd.MM.yyyy HH:mm", null,
                                System.Globalization.DateTimeStyles.None, out DateTime dueDate))
                        {
                            if (dueDate < DateTime.Now)
                            {
                                Console.WriteLine("\n| Ошибка: дата должна быть не раньше текущей даты! |");
                                continue;
                            }

                            int newTaskId = Requests.NewTask(userId, title, description, dueDate);
                            
                            if (newTaskId == -1)
                            {
                                Console.WriteLine("\n| Ошибка: Такая задача уже существует! |");
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("\n| Задача успешно добавлена! |");
                            }
                            
                            break;
                        }
                        
                        else if (DateTime.TryParseExact(dueDateString, "dd.MM.yyyy", null,
                                     System.Globalization.DateTimeStyles.None, out dueDate))
                        {
                            dueDate = dueDate.Date.AddHours(0).AddMinutes(0);
                            if (dueDate < DateTime.Now)
                            {
                                Console.WriteLine("\n| Ошибка: дата должна быть не раньше текущей даты |");
                                continue;
                            }

                            int newTaskId = Requests.NewTask(userId, title, description, dueDate);
        
                            if (newTaskId == -1)
                            {
                                Console.WriteLine("\n| Ошибка: Такая задача уже существует |");
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("\n| Задача успешно добавлена |");
                            }
        
                            break;
                        }
                        else
                        {
                            Console.WriteLine("\n| Ошибка: невозможно преобразовать данное значение в DateTime! |");
                            continue;
                        }
                    }

                    break;
                case 3:
                    allTasks = Requests.GetAllTasks(userId);
                    foreach (var task in allTasks)
                    {
                        Requests.PrintTitleAndIdTask(task);
                    }

                    int taskId =
                        StringToInt(
                            AntiEmptyStringMenu(
                                "\nВведите id задачи, которую нужно удалить (int): "));
                    
                    if (taskId == 0) continue;

                    int taskCheck = Requests.TaskCheck(userId, taskId);
                    if (taskCheck == -1)
                    {
                        Console.WriteLine("\n| Ошибка: данная задача - недоступна! |");
                    }
                    else
                    {
                        Requests.DeleteTask(taskId);
                        Console.WriteLine("\n| Задача успешно удалена! |");
                    }

                    break;
                case 4:
                    allTasks = Requests.GetAllTasks(userId);
                    foreach (var task in allTasks)
                    {
                        Requests.PrintTitleAndIdTask(task);
                    }

                    taskId =
                        StringToInt(
                            AntiEmptyStringMenu(
                                "\nВведите id задачи, которую нужно отредактировать (int): "));
                    
                    if (taskId == 0) continue;
                    
                    
                    taskCheck = Requests.TaskCheck(userId, taskId);
                    if (taskCheck == -1)
                    {
                        Console.WriteLine("\n| Ошибка: данная задача - недоступна! |");
                    }
                    else
                    {
                        var task = Requests.GetTask(taskId);
                        string titleEdit = task.Title;
                        string descriptionEdit = task.Description;
                        DateTime dueDateEdit = task.Duedate;
                        
                        bool EditMenuWork = true;
                        while (EditMenuWork)
                        {
                            int EditMenu =
                                StringToInt(
                                    AntiEmptyStringMenu(
                                        "1 - Изменить название задачи \n2 - Изменить описание задачи \n3 - Изменить сроки выполнения задачи \n4 - Назад \n\nВыберите нужное действие: "));
                            switch (EditMenu)
                            {
                                case 0:
                                    break;
                                case 1:
                                    titleEdit = AntiEmptyString("Введите название задачи (string): ");
                                    Requests.EditTask(taskId, titleEdit, descriptionEdit, dueDateEdit);
                                    Console.WriteLine("\n| Задача успешно отредактирована! |");
                                    
                                    EditMenuWork = false;
                                    break;
                                case 2:
                                    descriptionEdit = AntiEmptyString("Введите описание задачи (string): ");
                                    Requests.EditTask(taskId, titleEdit, descriptionEdit, dueDateEdit);
                                    Console.WriteLine("\n| Задача успешно отредактирована! |");
                                    
                                    EditMenuWork = false;
                                    break;
                                case 3:
                                    while (true)
                                    {
                                        string dueDateString = AntiEmptyString("Введите дату и время (в формате ДД.ММ.ГГГГ ЧЧ:ММ): ");

                                        if (DateTime.TryParseExact(dueDateString, "dd.MM.yyyy HH:mm", null,
                                                System.Globalization.DateTimeStyles.None, out dueDateEdit))
                                        {
                                            if (dueDateEdit < DateTime.Now)
                                            {
                                                Console.WriteLine("\n| Ошибка: дата должна быть не раньше текущей даты! |");
                                                continue;
                                            }

                                            Requests.EditTask(taskId, titleEdit, descriptionEdit, dueDateEdit);
                                            Console.WriteLine("\n| Задача успешно отредактирована! |");
                                            
                                            EditMenuWork = false;
                                            break;
                                        }
                                        
                                        else if (DateTime.TryParseExact(dueDateString, "dd.MM.yyyy", null,
                                                     System.Globalization.DateTimeStyles.None, out dueDateEdit))
                                        {
                                            dueDateEdit = dueDateEdit.Date.AddHours(0).AddMinutes(0);
                                            
                                            if (dueDateEdit < DateTime.Now)
                                            {
                                                Console.WriteLine("\n| Ошибка: дата должна быть не раньше текущей даты! |");
                                                continue;
                                            }

                                            Requests.EditTask(taskId, titleEdit, descriptionEdit, dueDateEdit);
                                            Console.WriteLine("\n| Задача успешно отредактирована! |");
                                            
                                            EditMenuWork = false;
                                            break;
                                        }
                                        
                                        else
                                        {
                                            Console.WriteLine("\n| Ошибка: невозможно преобразовать данное значение в DateTime! |");
                                        }
                                    }
                                    
                                    break;
                                case 4:
                                    EditMenuWork = false;
                                    break;
                                default:
                                    Console.WriteLine("\n| Неизвестное действие! |");
                                    break;
                            }
                        }
                    }

                    break;
                case 5:
                    mainMenuWork = false;
                    break;
                default:
                    Console.WriteLine("\n| Неизвестное действие! |");
                    break;
            }
        } 
    }

    public static int StringToInt(string inputStr)
    {
        int input = 0;
        try
        {
            input = Convert.ToInt32(inputStr);
        }

        catch (FormatException)
        {
            Console.WriteLine();
            Console.WriteLine("\n| Ошибка: невозможно преобразовать данное значение в int |");
        }

        return input;
    }

    public static string AntiEmptyString(string inputText)
    {
        string input;
        while (true)
        {
            Console.Write(inputText);
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) break;
        }

        return input;
    }

    public static string AntiEmptyStringMenu(string inputText)
    {
        string input;
        while (true)
        {
            Console.WriteLine();
            Console.Write(inputText);
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) break;
        }

        return input;
    }
}