class Program
{
    static void Main()
    {

        Logger logger = new Logger();
        logger.Log("Ошибка подключения", LogLevel.Error);

        DatabaseService db = new DatabaseService();
        db.Connect();

        NumberProcessor np = new NumberProcessor();
        np.ProcessNumbers(new int[] { -1, 0, 5, 10 });

        User user = new User { Name = "Merey", Email = "mereidosaibek@gmail.com" };
        user.SaveToDatabase();
    }
}
