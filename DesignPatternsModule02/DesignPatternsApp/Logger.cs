public enum LogLevel { Error, Warning, Info }

public class Logger
{
    public void Log(string message, LogLevel level)
    {
        Console.WriteLine($"{level}: {message}");
    }
}
