using System;
using System.Collections.Generic;

interface IPaymentStrategy { void Pay(decimal amount); }

class CardPayment : IPaymentStrategy { public void Pay(decimal amount) => Console.WriteLine($"Оплата картой: {amount}₸"); }
class PayPalPayment : IPaymentStrategy { public void Pay(decimal amount) => Console.WriteLine($"Оплата через PayPal: {amount}₸"); }
class CryptoPayment : IPaymentStrategy { public void Pay(decimal amount) => Console.WriteLine($"Оплата криптовалютой: {amount}₸"); }

class PaymentContext
{
    IPaymentStrategy strategy;
    public void SetStrategy(IPaymentStrategy s) => strategy = s;
    public void ExecutePayment(decimal amount) => strategy.Pay(amount);
}

interface IObserver { void Update(string currency, decimal rate); }
interface ISubject { void Add(IObserver o); void Remove(IObserver o); void Notify(); }

class CurrencyExchange : ISubject
{
    List<IObserver> observers = new();
    string currency; decimal rate;
    public void Add(IObserver o) => observers.Add(o);
    public void Remove(IObserver o) => observers.Remove(o);
    public void SetRate(string c, decimal r) { currency = c; rate = r; Notify(); }
    public void Notify() { foreach (var o in observers) o.Update(currency, rate); }
}

class Bank : IObserver { public void Update(string c, decimal r) => Console.WriteLine($"Банк: курс {c} = {r}"); }
class Broker : IObserver { public void Update(string c, decimal r) => Console.WriteLine($"Брокер: обновление {c} = {r}"); }
class MobileApp : IObserver { public void Update(string c, decimal r) => Console.WriteLine($"Мобильное приложение: {c} = {r}"); }

class Program
{
    static void Main()
    {
        var context = new PaymentContext();
        Console.WriteLine("Выберите оплату: 1-Карта 2-PayPal 3-Крипто");
        var choice = Console.ReadLine();
        if (choice == "1") context.SetStrategy(new CardPayment());
        else if (choice == "2") context.SetStrategy(new PayPalPayment());
        else context.SetStrategy(new CryptoPayment());
        context.ExecutePayment(1000);

        var exchange = new CurrencyExchange();
        var bank = new Bank(); var broker = new Broker(); var app = new MobileApp();
        exchange.Add(bank); exchange.Add(broker); exchange.Add(app);
        exchange.SetRate("USD", 480.5m);
        exchange.Remove(broker);
        exchange.SetRate("EUR", 510.2m);
    }
}
