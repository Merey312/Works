using System;

public class CreditCardPayment : IPaymentMethod
{
    public void Process(decimal amount)
    {
        Console.WriteLine($"Processing credit card payment of {amount}");
    }
}
