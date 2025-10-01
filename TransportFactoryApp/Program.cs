using System;

public interface IVehicle
{
    void Drive();
    void Refuel();
}

public class Car : IVehicle
{
    private string Brand;
    private string Model;
    private string Fuel;

    public Car(string brand, string model, string fuel)
    {
        Brand = brand;
        Model = model;
        Fuel = fuel;
    }

    public void Drive() => Console.WriteLine($"Автомобиль {Brand} {Model} едет на {Fuel}.");
    public void Refuel() => Console.WriteLine($"Заправляем {Fuel} для {Brand} {Model}.");
}

public class Motorcycle : IVehicle
{
    private string Type;
    private int EngineVolume;

    public Motorcycle(string type, int engineVolume)
    {
        Type = type;
        EngineVolume = engineVolume;
    }

    public void Drive() => Console.WriteLine($"{Type}-мотоцикл ({EngineVolume}cc) едет.");
    public void Refuel() => Console.WriteLine($"Заправляем мотоцикл ({EngineVolume}cc).");
}

public class Truck : IVehicle
{
    private int Capacity;
    private int Axles;

    public Truck(int capacity, int axles)
    {
        Capacity = capacity;
        Axles = axles;
    }

    public void Drive() => Console.WriteLine($"Грузовик {Capacity} т, осей: {Axles} – в пути.");
    public void Refuel() => Console.WriteLine("Заправляем дизельный грузовик.");
}

public class Bus : IVehicle
{
    private int Seats;
    public Bus(int seats) => Seats = seats;

    public void Drive() => Console.WriteLine($"Автобус на {Seats} мест едет по маршруту.");
    public void Refuel() => Console.WriteLine("Заправляем автобус.");
}

public abstract class VehicleFactory
{
    public abstract IVehicle CreateVehicle();
}

// Конкретные фабрики
public class CarFactory : VehicleFactory
{
    private string Brand, Model, Fuel;
    public CarFactory(string brand, string model, string fuel)
    {
        Brand = brand; Model = model; Fuel = fuel;
    }
    public override IVehicle CreateVehicle() => new Car(Brand, Model, Fuel);
}

public class MotorcycleFactory : VehicleFactory
{
    private string Type; private int EngineVolume;
    public MotorcycleFactory(string type, int engineVolume)
    {
        Type = type; EngineVolume = engineVolume;
    }
    public override IVehicle CreateVehicle() => new Motorcycle(Type, EngineVolume);
}

public class TruckFactory : VehicleFactory
{
    private int Capacity, Axles;
    public TruckFactory(int capacity, int axles)
    {
        Capacity = capacity; Axles = axles;
    }
    public override IVehicle CreateVehicle() => new Truck(Capacity, Axles);
}

public class BusFactory : VehicleFactory
{
    private int Seats;
    public BusFactory(int seats) => Seats = seats;
    public override IVehicle CreateVehicle() => new Bus(Seats);
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Выберите транспорт: 1-Авто, 2-Мото, 3-Грузовик, 4-Автобус");
        string choice = Console.ReadLine();

        VehicleFactory factory = choice switch
        {
            "1" => new CarFactory("Toyota", "Camry", "бензин"),
            "2" => new MotorcycleFactory("Спортивный", 600),
            "3" => new TruckFactory(10, 4),
            "4" => new BusFactory(50),
            _   => null
        };

        if (factory == null)
        {
            Console.WriteLine("Неверный выбор!");
            return;
        }

        IVehicle vehicle = factory.CreateVehicle();
        vehicle.Drive();
        vehicle.Refuel();
    }
}

