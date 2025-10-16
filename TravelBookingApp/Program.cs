using System;

interface ICostCalculationStrategy
{
    decimal CalculateCost(decimal distance, string serviceClass, int passengers, bool hasDiscount);
}

class PlaneStrategy : ICostCalculationStrategy
{
    public decimal CalculateCost(decimal distance, string serviceClass, int passengers, bool hasDiscount)
    {
        decimal baseCost = distance * 0.7m * passengers;
        if (serviceClass == "бизнес") baseCost *= 1.5m;
        if (hasDiscount) baseCost *= 0.9m;
        return baseCost;
    }
}

class TrainStrategy : ICostCalculationStrategy
{
    public decimal CalculateCost(decimal distance, string serviceClass, int passengers, bool hasDiscount)
    {
        decimal baseCost = distance * 0.3m * passengers;
        if (serviceClass == "бизнес") baseCost *= 1.3m;
        if (hasDiscount) baseCost *= 0.85m;
        return baseCost;
    }
}

class BusStrategy : ICostCalculationStrategy
{
    public decimal CalculateCost(decimal distance, string serviceClass, int passengers, bool hasDiscount)
    {
        decimal baseCost = distance * 0.2m * passengers;
        if (serviceClass == "бизнес") baseCost *= 1.2m;
        if (hasDiscount) baseCost *= 0.8m;
        return baseCost;
    }
}

class TravelBookingContext
{
    private ICostCalculationStrategy _strategy;
    public void SetStrategy(ICostCalculationStrategy strategy) => _strategy = strategy;
    public decimal Calculate(decimal distance, string serviceClass, int passengers, bool hasDiscount)
    {
        if (_strategy == null) throw new InvalidOperationException("Стратегия не выбрана");
        return _strategy.CalculateCost(distance, serviceClass, passengers, hasDiscount);
    }
}

class Program
{
    static void Main()
    {
        var ctx = new TravelBookingContext();

        Console.WriteLine("Выберите транспорт: 1 - Самолет, 2 - Поезд, 3 - Автобус");
        string choice = Console.ReadLine();

        ctx.SetStrategy(choice switch
        {
            "1" => new PlaneStrategy(),
            "2" => new TrainStrategy(),
            "3" => new BusStrategy(),
            _ => throw new Exception("Неверный выбор")
        });

        Console.Write("Расстояние (км): ");
        decimal distance = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Количество пассажиров: ");
        int passengers = Convert.ToInt32(Console.ReadLine());

        Console.Write("Класс обслуживания (эконом/бизнес): ");
        string serviceClass = Console.ReadLine().ToLower();

        Console.Write("Есть скидка (y/n): ");
        bool discount = Console.ReadLine().ToLower() == "y";

        decimal cost = ctx.Calculate(distance, serviceClass, passengers, discount);
        Console.WriteLine($"Стоимость поездки: {cost:C}");
    }
}
