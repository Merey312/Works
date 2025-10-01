using System;

class Program
{
    static void Main()
    {
        
        Log("Ошибка подключения", "ERROR");
        Log("Успешное подключение", "INFO");

        
        int[] numbers = { -1, 0, 5, 10 };
        foreach (int n in numbers)
        {
            if (n > 0)
                Console.WriteLine($"{n} – положительное");
            else if (n < 0)
                Console.WriteLine($"{n} – отрицательное");
            else
                Console.WriteLine($"{n} – ноль");
        }

        
        User user = new User("Merey", "merey@gmail.com");
        user.Save();
    }

    
    static void Log(string message, string level)
    {
        Console.WriteLine($"[{level}] {message}");
    }
}


class User
{
    public string Name { get; set; }
    public string Email { get; set; }

    public User(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public void Save()
    {
        Console.WriteLine($"Пользователь {Name} сохранён с email {Email}");
    }
}
