using System;

public class PayPalPayment : IPaymentMethod
{
    public void Process(decimal amount)
    {
        Console.WriteLine($"Processing PayPal payment of {amount}");
    }
}
