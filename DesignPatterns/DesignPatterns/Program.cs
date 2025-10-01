using System;
using System.Collections.Generic;

public class Item
{
    public string Name { get; set; }
    public double Price { get; set; }
}

public class Invoice
{
    public int Id { get; set; }
    public List<Item> Items { get; set; }
    public double TaxRate { get; set; }
}

public class InvoiceCalculator
{
    public double CalculateTotal(Invoice invoice)
    {
        double subTotal = 0;
        foreach (var item in invoice.Items)
        {
            subTotal += item.Price;
        }
        return subTotal + (subTotal * invoice.TaxRate);
    }
}
public class InvoiceRepository
{
    public void SaveToDatabase(Invoice invoice)
    {
        Console.WriteLine($"Invoice {invoice.Id} сохранен в базу данных.");
    }
}
public abstract class DiscountStrategy
{
    public abstract double ApplyDiscount(double amount);
}

public class RegularDiscount : DiscountStrategy
{
    public override double ApplyDiscount(double amount) => amount;
}

public class SilverDiscount : DiscountStrategy
{
    public override double ApplyDiscount(double amount) => amount * 0.9;
}

public class GoldDiscount : DiscountStrategy
{
    public override double ApplyDiscount(double amount) => amount * 0.8;
}

public class PlatinumDiscount : DiscountStrategy
{
    public override double ApplyDiscount(double amount) => amount * 0.7;
}

public class DiscountCalculator
{
    private readonly DiscountStrategy _strategy;

    public DiscountCalculator(DiscountStrategy strategy)
    {
        _strategy = strategy;
    }

    public double Calculate(double amount) => _strategy.ApplyDiscount(amount);
}

public interface IWorkable
{
    void Work();
}

public interface IEatable
{
    void Eat();
}

public interface ISleepable
{
    void Sleep();
}

public class HumanWorker : IWorkable, IEatable, ISleepable
{
    public void Work() => Console.WriteLine("Человек работает");
    public void Eat() => Console.WriteLine("Человек ест");
    public void Sleep() => Console.WriteLine("Человек спит");
}

public class RobotWorker : IWorkable
{
    public void Work() => Console.WriteLine("Робот работает");
}

// ===== DIP =====
public interface IMessageService
{
    void Send(string message);
}

public class EmailService : IMessageService
{
    public void Send(string message) => Console.WriteLine($"Отправка Email: {message}");
}

public class SmsService : IMessageService
{
    public void Send(string message) => Console.WriteLine($"Отправка SMS: {message}");
}

public class Notification
{
    private readonly IMessageService _messageService;

    public Notification(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public void Send(string message)
    {
        _messageService.Send(message);
    }
}

class Program
{
    static void Main()
    {
        // SRP
        var invoice = new Invoice
        {
            Id = 1,
            Items = new List<Item> { new Item { Name = "Товар1", Price = 100 }, new Item { Name = "Товар2", Price = 200 } },
            TaxRate = 0.2
        };
        var calculator = new InvoiceCalculator();
        var total = calculator.CalculateTotal(invoice);
        Console.WriteLine($"Итого по счету: {total}");
        var repo = new InvoiceRepository();
        repo.SaveToDatabase(invoice);

        // OCP
        var discount = new DiscountCalculator(new GoldDiscount());
        Console.WriteLine($"Сумма со скидкой: {discount.Calculate(1000)}");

        // ISP
        IWorkable worker1 = new HumanWorker();
        worker1.Work();
        IWorkable worker2 = new RobotWorker();
        worker2.Work();

        // DIP
        var notification = new Notification(new EmailService());
        notification.Send("Ваш заказ подтвержден!");

        var smsNotification = new Notification(new SmsService());
        smsNotification.Send("Ваш заказ доставлен!");
    }
}
