using System;
using System.Collections.Generic;

interface IObserver
{
    void Update(string stock, decimal price);
}

interface ISubject
{
    void AddObserver(string stock, IObserver observer);
    void RemoveObserver(string stock, IObserver observer);
    void NotifyObservers(string stock, decimal price);
}

class StockExchange : ISubject
{
    private readonly Dictionary<string, List<IObserver>> _observers = new();
    private readonly Dictionary<string, decimal> _stocks = new();

    public void AddStock(string name, decimal price)
    {
        _stocks[name] = price;
        _observers[name] = new List<IObserver>();
    }

    public void UpdatePrice(string stock, decimal newPrice)
    {
        if (!_stocks.ContainsKey(stock)) return;
        _stocks[stock] = newPrice;
        NotifyObservers(stock, newPrice);
    }

    public void AddObserver(string stock, IObserver observer)
    {
        if (!_observers.ContainsKey(stock)) _observers[stock] = new List<IObserver>();
        _observers[stock].Add(observer);
    }

    public void RemoveObserver(string stock, IObserver observer)
    {
        _observers[stock]?.Remove(observer);
    }

    public void NotifyObservers(string stock, decimal price)
    {
        if (!_observers.ContainsKey(stock)) return;
        foreach (var obs in _observers[stock]) obs.Update(stock, price);
    }
}

class Trader : IObserver
{
    private readonly string _name;
    public Trader(string name) => _name = name;
    public void Update(string stock, decimal price)
    {
        Console.WriteLine($"{_name} получил обновление: {stock} = {price:C}");
    }
}

class AutoBot : IObserver
{
    public void Update(string stock, decimal price)
    {
        if (price < 100) Console.WriteLine($"[Bot] Покупаю {stock} по цене {price:C}");
        else Console.WriteLine($"[Bot] Продаю {stock} по цене {price:C}");
    }
}

class EmailNotifier : IObserver
{
    public void Update(string stock, decimal price)
    {
        Console.WriteLine($"[Email] Уведомление: {stock} сейчас стоит {price:C}");
    }
}

class Program
{
    static void Main()
    {
        var exchange = new StockExchange();
        exchange.AddStock("AAPL", 150);
        exchange.AddStock("TSLA", 200);

        var trader = new Trader("Али");
        var bot = new AutoBot();
        var mail = new EmailNotifier();

        exchange.AddObserver("AAPL", trader);
        exchange.AddObserver("AAPL", bot);
        exchange.AddObserver("TSLA", mail);
        exchange.AddObserver("TSLA", bot);

        exchange.UpdatePrice("AAPL", 120);
        exchange.UpdatePrice("TSLA", 95);
        exchange.UpdatePrice("TSLA", 210);
    }
}
