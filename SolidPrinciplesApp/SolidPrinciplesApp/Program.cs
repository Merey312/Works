using System;
using System.Collections.Generic;

public class Order
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
}

public class OrderCalculator
{
    public double CalculateTotalPrice(Order order)
    {
        return order.Quantity * order.Price * 0.9; 
    }
}


public class PaymentProcessor
{
    public void ProcessPayment(string paymentDetails)
    {
        Console.WriteLine("Payment processed using: " + paymentDetails);
    }
}

public class EmailService
{
    public void SendConfirmationEmail(string email)
    {
        Console.WriteLine("Confirmation email sent to: " + email);
    }
}


//  OCP 
public abstract class Employee
{
    public string Name { get; set; }
    public double BaseSalary { get; set; }

    public abstract double CalculateSalary();
}

public class PermanentEmployee : Employee
{
    public override double CalculateSalary()
    {
        return BaseSalary * 1.2;
    }
}

public class ContractEmployee : Employee
{
    public override double CalculateSalary()
    {
        return BaseSalary * 1.1;
    }
}

public class Intern : Employee
{
    public override double CalculateSalary()
    {
        return BaseSalary * 0.8;
    }
}

public class Freelancer : Employee
{
    public override double CalculateSalary()
    {
        return BaseSalary * 0.9; 
    }
}


// ISP 
public interface IPrinter
{
    void Print(string content);
}

public interface IScanner
{
    void Scan(string content);
}

public interface IFax
{
    void Fax(string content);
}


public class AllInOnePrinter : IPrinter, IScanner, IFax
{
    public void Print(string content)
    {
        Console.WriteLine("Printing: " + content);
    }

    public void Scan(string content)
    {
        Console.WriteLine("Scanning: " + content);
    }

    public void Fax(string content)
    {
        Console.WriteLine("Faxing: " + content);
    }
}


public class BasicPrinter : IPrinter
{
    public void Print(string content)
    {
        Console.WriteLine("Printing: " + content);
    }
}

public class PrinterScanner : IPrinter, IScanner
{
    public void Print(string content)
    {
        Console.WriteLine("Printing: " + content);
    }

    public void Scan(string content)
    {
        Console.WriteLine("Scanning: " + content);
    }
}


class Program
{
    static void Main()
    {
        Console.WriteLine("=== SRP Example ===");
        Order order = new Order { ProductName = "Laptop", Quantity = 2, Price = 1000 };
        OrderCalculator calculator = new OrderCalculator();
        PaymentProcessor payment = new PaymentProcessor();
        EmailService emailService = new EmailService();

        Console.WriteLine($"Total Price: {calculator.CalculateTotalPrice(order)}");
        payment.ProcessPayment("Credit Card");
        emailService.SendConfirmationEmail("merey@gmail.com");

        Console.WriteLine("\n=== OCP Example ===");
        List<Employee> employees = new List<Employee>
        {
            new PermanentEmployee { Name = "Merey", BaseSalary = 1000 },
            new ContractEmployee { Name = "Altynay", BaseSalary = 1000 },
            new Intern { Name = "Ali", BaseSalary = 1000 },
            new Freelancer { Name = "Emir", BaseSalary = 1000 }
        };

        foreach (var emp in employees)
        {
            Console.WriteLine($"{emp.Name} salary: {emp.CalculateSalary()}");
        }

        Console.WriteLine("\n=== ISP Example ===");
        IPrinter basic = new BasicPrinter();
        basic.Print("Simple document");

        AllInOnePrinter allInOne = new AllInOnePrinter();
        allInOne.Print("Report");
        allInOne.Scan("Photo");
        allInOne.Fax("Contract");

        PrinterScanner ps = new PrinterScanner();
        ps.Print("Invoice");
        ps.Scan("Passport");
    }
}
