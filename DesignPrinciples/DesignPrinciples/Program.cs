using System;
public class OrderService
{
    private double CalculateTotal(int quantity, double price) => quantity * price;

    public void CreateOrder(string productName, int quantity, double price)
    {
        double totalPrice = CalculateTotal(quantity, price);
        Console.WriteLine($"Order for {productName} created. Total: {totalPrice}");
    }

    public void UpdateOrder(string productName, int quantity, double price)
    {
        double totalPrice = CalculateTotal(quantity, price);
        Console.WriteLine($"Order for {productName} updated. New total: {totalPrice}");
    }
}

public abstract class Vehicle
{
    public virtual void Start() => Console.WriteLine($"{GetType().Name} is starting");
    public virtual void Stop() => Console.WriteLine($"{GetType().Name} is stopping");
}

public class Car : Vehicle { }
public class Truck : Vehicle { }


public class Calculator
{
    public void Add(int a, int b)
    {
        Console.WriteLine($"Result: {a + b}");
    }
}

public sealed class SimpleSingleton
{
    private static readonly SimpleSingleton _instance = new SimpleSingleton();
    private SimpleSingleton() { }
    public static SimpleSingleton Instance => _instance;

    public void DoSomething()
    {
        Console.WriteLine("Doing something in a simple way...");
    }
}


public class Circle
{
    private double _radius;
    public Circle(double radius) => _radius = radius;

    public double CalculateArea() => Math.PI * _radius * _radius;
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== DRY ===");
        var orderService = new OrderService();
        orderService.CreateOrder("Laptop", 2, 500);
        orderService.UpdateOrder("Phone", 3, 200);

        Vehicle car = new Car();
        car.Start();
        car.Stop();

        Vehicle truck = new Truck();
        truck.Start();
        truck.Stop();

        Console.WriteLine("\n=== KISS ===");
        var calc = new Calculator();
        calc.Add(5, 10);

        SimpleSingleton.Instance.DoSomething();

        Console.WriteLine("\n=== YAGNI ===");
        var circle = new Circle(5);
        Console.WriteLine($"Circle area: {circle.CalculateArea()}");
    }
}
