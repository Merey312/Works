using System;

public class BankTransferPayment : IPaymentMethod
{
    public void Process(decimal amount)
    {
        Console.WriteLine($"Processing bank transfer payment of {amount}");
    }
}
