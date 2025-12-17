public class PaymentProcessor
{
    private readonly IPaymentMethod paymentMethod;

    public PaymentProcessor(IPaymentMethod paymentMethod)
    {
        this.paymentMethod = paymentMethod;
    }

    public void ProcessPayment(decimal amount)
    {
        paymentMethod.Process(amount);
    }
}
