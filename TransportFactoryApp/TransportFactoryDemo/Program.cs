using System;

public interface ITransport
{
    void Move();
    void FuelUp();
}

public class Car : ITransport
{
    private string Model;
    private int Speed;

    public Car(string model, int speed)
    {
        Model = model;
        Speed = speed;
    }

    public void Move() => Console.WriteLine($"🚗 Автомобиль {Model} едет со скоростью {Speed} км/ч.");
    public void FuelUp() => Console.WriteLine($"Заправляем автомобиль {Model} бензином.");
}

public class Motorcycle : ITransport
{
    private string Model;
    private int Speed;

    public Motorcycle(string model, int speed)
    {
        Model = model;
        Speed = speed;
    }

    public void Move() => Console.WriteLine($"🏍 Мотоцикл {Model} едет со скоростью {Speed} км/ч.");
    public void FuelUp() => Console.WriteLine($"Заправляем мотоцикл {Model} бензином.");
}

public class Plane : ITransport
{
    private string Model;
    private int Speed;

    public Plane(string model, int speed)
    {
        Model = model;
        Speed = speed;
    }

    public void Move() => Console.WriteLine($"✈ Самолет {Model} летит со скоростью {Speed} км/ч.");
    public void FuelUp() => Console.WriteLine($"Заправляем самолет {Model} авиационным топливом.");
}

// Новый тип: Велосипед
public class Bicycle : ITransport
{
    private string Model;
    private int Speed;

    public Bicycle(string model, int speed)
    {
        Model = model;
        Speed = speed;
    }

    public void Move() => Console.WriteLine($"🚴 Велосипед {Model} движется со скоростью {Speed} км/ч.");
    public void FuelUp() => Console.WriteLine("Велосипед не требует топлива, только силы ног! 💪");
}

public abstract class TransportFactory
{
    public abstract ITransport CreateTransport(string model, int speed);
}

public class CarFactory : TransportFactory
{
    public override ITransport CreateTransport(string model, int speed) => new Car(model, speed);
}

public class MotorcycleFactory : TransportFactory
{
    public override ITransport CreateTransport(string model, int speed) => new Motorcycle(model, speed);
}

public class PlaneFactory : TransportFactory
{
    public override ITransport CreateTransport(string model, int speed) => new Plane(model, speed);
}

public class BicycleFactory : TransportFactory
{
    public override ITransport CreateTransport(string model, int speed) => new Bicycle(model, speed);
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Выберите транспорт: 1 - Авто, 2 - Мотоцикл, 3 - Самолет, 4 - Велосипед");
        string choice = Console.ReadLine();

        Console.Write("Введите модель: ");
        string model = Console.ReadLine();

        Console.Write("Введите скорость: ");
        int speed = int.Parse(Console.ReadLine());

        TransportFactory factory = choice switch
        {
            "1" => new CarFactory(),
            "2" => new MotorcycleFactory(),
            "3" => new PlaneFactory(),
            "4" => new BicycleFactory(),
            _ => null
        };

        if (factory == null)
        {
            Console.WriteLine("Ошибка: неверный выбор!");
            return;
        }

        ITransport transport = factory.CreateTransport(model, speed);
        transport.Move();
        transport.FuelUp();
    }
}
