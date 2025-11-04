using System;
abstract class Beverage
{
    public abstract string Description { get; }
    public abstract double Cost();
}

class Espresso : Beverage
{
    public override string Description => "Espresso";
    public override double Cost() => 300;
}
class Tea : Beverage
{
    public override string Description => "Tea";
    public override double Cost() => 200;
}
class Latte : Beverage
{
    public override string Description => "Latte";
    public override double Cost() => 350;
}

abstract class BeverageDecorator : Beverage
{
    protected Beverage beverage;
    public BeverageDecorator(Beverage b) => beverage = b;
}

class Milk : BeverageDecorator
{
    public Milk(Beverage b) : base(b) { }
    public override string Description => beverage.Description + " + Milk";
    public override double Cost() => beverage.Cost() + 50;
}
class Sugar : BeverageDecorator
{
    public Sugar(Beverage b) : base(b) { }
    public override string Description => beverage.Description + " + Sugar";
    public override double Cost() => beverage.Cost() + 30;
}
class WhippedCream : BeverageDecorator
{
    public WhippedCream(Beverage b) : base(b) { }
    public override string Description => beverage.Description + " + Whipped Cream";
    public override double Cost() => beverage.Cost() + 60;
}


interface IPaymentProcessor
{
    void ProcessPayment(double amount);
}

class PayPalPaymentProcessor : IPaymentProcessor
{
    public void ProcessPayment(double amount)
    {
        Console.WriteLine($"PayPal оплатил {amount} тг");
    }
}

class StripePaymentService
{
    public void MakeTransaction(double totalAmount)
    {
        Console.WriteLine($"Stripe транзакция на {totalAmount} тг");
    }
}

class StripePaymentAdapter : IPaymentProcessor
{
    private StripePaymentService stripe = new StripePaymentService();
    public void ProcessPayment(double amount)
    {
        stripe.MakeTransaction(amount);
    }
}

class Program
{
    static void Main()
    {
        
        Beverage drink = new Latte();
        drink = new Milk(drink);
        drink = new Sugar(drink);
        drink = new WhippedCream(drink);

        Console.WriteLine("Напиток: " + drink.Description);
        Console.WriteLine("Цена: " + drink.Cost() + " тг\n");


        
        IPaymentProcessor paypal = new PayPalPaymentProcessor();
        IPaymentProcessor stripe = new StripePaymentAdapter();

        paypal.ProcessPayment(drink.Cost());
        stripe.ProcessPayment(drink.Cost());
    }
}
