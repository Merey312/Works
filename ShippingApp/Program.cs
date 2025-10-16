using System;

interface IShippingStrategy
{
    decimal CalculateShippingCost(decimal weight, decimal distance);
}

class StandardShippingStrategy : IShippingStrategy
{
    public decimal CalculateShippingCost(decimal weight, decimal distance)
    {
        return weight * 0.5m + distance * 0.1m;
    }
}

class ExpressShippingStrategy : IShippingStrategy
{
    public decimal CalculateShippingCost(decimal weight, decimal distance)
    {
        return (weight * 0.75m + distance * 0.2m) + 10m;
    }
}

class InternationalShippingStrategy : IShippingStrategy
{
    public decimal CalculateShippingCost(decimal weight, decimal distance)
    {
        return weight * 1.0m + distance * 0.5m + 15m;
    }
}

class NightShippingStrategy : IShippingStrategy
{
    private readonly IShippingStrategy _baseStrategy;
    private readonly decimal _nightSurcharge;

    public NightShippingStrategy(IShippingStrategy baseStrategy, decimal nightSurcharge = 20m)
    {
        _baseStrategy = baseStrategy ?? throw new ArgumentNullException(nameof(baseStrategy));
        _nightSurcharge = nightSurcharge;
    }

    public decimal CalculateShippingCost(decimal weight, decimal distance)
    {
        return _baseStrategy.CalculateShippingCost(weight, distance) + _nightSurcharge;
    }
}

class DeliveryContext
{
    private IShippingStrategy _shippingStrategy;

    public void SetShippingStrategy(IShippingStrategy strategy)
    {
        _shippingStrategy = strategy;
    }

    public decimal CalculateCost(decimal weight, decimal distance)
    {
        if (_shippingStrategy == null) throw new InvalidOperationException("Стратегия доставки не установлена.");
        return _shippingStrategy.CalculateShippingCost(weight, distance);
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Расчёт стоимости доставки (Strategy) ===");

        var ctx = new DeliveryContext();

        Console.WriteLine("Выберите тип доставки:");
        Console.WriteLine("1 - Стандартная");
        Console.WriteLine("2 - Экспресс");
        Console.WriteLine("3 - Международная");
        Console.Write("Выбор (1/2/3): ");
        string choice = Console.ReadLine()?.Trim();

        IShippingStrategy baseStrategy = choice switch
        {
            "1" => new StandardShippingStrategy(),
            "2" => new ExpressShippingStrategy(),
            "3" => new InternationalShippingStrategy(),
            _ => null
        };

        if (baseStrategy == null)
        {
            Console.WriteLine("Неверный выбор стратегии. Выход.");
            return;
        }

        Console.Write("Нужна ночная доставка? (y/n): ");
        string night = Console.ReadLine()?.Trim().ToLower();
        if (night == "y" || night == "yes")
        {
            ctx.SetShippingStrategy(new NightShippingStrategy(baseStrategy));
        }
        else
        {
            ctx.SetShippingStrategy(baseStrategy);
        }

        decimal weight = ReadDecimal("Введите вес посылки (кг): ");
        decimal distance = ReadDecimal("Введите расстояние доставки (км): ");

        try
        {
            decimal cost = ctx.CalculateCost(weight, distance);
            Console.WriteLine($"Стоимость доставки: {cost:C}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при расчёте: {ex.Message}");
        }
    }

    static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim();
            if (decimal.TryParse(input, out decimal value))
            {
                if (value < 0)
                {
                    Console.WriteLine("Значение не может быть отрицательным. Попробуйте ещё раз.");
                    continue;
                }
                return value;
            }
            Console.WriteLine("Неверный ввод. Введите число (например, 12.5).");
        }
    }
}
