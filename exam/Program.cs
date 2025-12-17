using System;

class Program
{
    static void Main(string[] args)
    {
        PaymentProcessor creditCardProcessor =
            new PaymentProcessor(new CreditCardPayment());
        creditCardProcessor.ProcessPayment(100);

        PaymentProcessor payPalProcessor =
            new PaymentProcessor(new PayPalPayment());
        payPalProcessor.ProcessPayment(200);

        PaymentProcessor bankTransferProcessor =
            new PaymentProcessor(new BankTransferPayment());
        bankTransferProcessor.ProcessPayment(300);

        Console.ReadLine();
    }
}
