public class DatabaseService
{
    private string connectionString = "Server=myServer;Database=myDb;User Id=merey;Password=qwerty;";

    public void Connect()
    {
        Console.WriteLine($"Подключение к базе: {connectionString}");
    }
}
