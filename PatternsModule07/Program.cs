using System;
using System.Collections.Generic;

interface ICommand
{
    void Execute();
    void Undo();
}

class Light
{
    public bool IsOn { get; private set; }
    public void TurnOn() { IsOn = true; Console.WriteLine("Light: ON"); }
    public void TurnOff() { IsOn = false; Console.WriteLine("Light: OFF"); }
}

class Door
{
    public bool IsOpen { get; private set; }
    public void Open() { IsOpen = true; Console.WriteLine("Door: OPEN"); }
    public void Close() { IsOpen = false; Console.WriteLine("Door: CLOSED"); }
}

class Thermostat
{
    public int Temperature { get; private set; } = 22;
    public void Increase(int delta) { Temperature += delta; Console.WriteLine($"Thermostat: {Temperature}°C"); }
    public void Decrease(int delta) { Temperature -= delta; Console.WriteLine($"Thermostat: {Temperature}°C"); }
}

class LightOnCommand : ICommand
{
    private readonly Light _light;
    public LightOnCommand(Light light) => _light = light ?? throw new ArgumentNullException(nameof(light));
    public void Execute() => _light.TurnOn();
    public void Undo() => _light.TurnOff();
}

class LightOffCommand : ICommand
{
    private readonly Light _light;
    public LightOffCommand(Light light) => _light = light ?? throw new ArgumentNullException(nameof(light));
    public void Execute() => _light.TurnOff();
    public void Undo() => _light.TurnOn();
}

class DoorOpenCommand : ICommand
{
    private readonly Door _door;
    public DoorOpenCommand(Door door) => _door = door ?? throw new ArgumentNullException(nameof(door));
    public void Execute() => _door.Open();
    public void Undo() => _door.Close();
}

class DoorCloseCommand : ICommand
{
    private readonly Door _door;
    public DoorCloseCommand(Door door) => _door = door ?? throw new ArgumentNullException(nameof(door));
    public void Execute() => _door.Close();
    public void Undo() => _door.Open();
}

class IncreaseTempCommand : ICommand
{
    private readonly Thermostat _thermo;
    private readonly int _delta;
    public IncreaseTempCommand(Thermostat t, int delta) { _thermo = t ?? throw new ArgumentNullException(nameof(t)); _delta = delta; }
    public void Execute() => _thermo.Increase(_delta);
    public void Undo() => _thermo.Decrease(_delta);
}

class DecreaseTempCommand : ICommand
{
    private readonly Thermostat _thermo;
    private readonly int _delta;
    public DecreaseTempCommand(Thermostat t, int delta) { _thermo = t ?? throw new ArgumentNullException(nameof(t)); _delta = delta; }
    public void Execute() => _thermo.Decrease(_delta);
    public void Undo() => _thermo.Increase(_delta);
}

class Invoker
{
    private readonly Stack<ICommand> _history = new Stack<ICommand>();

    public void ExecuteCommand(ICommand command)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));
        command.Execute();
        _history.Push(command);
    }

    public void UndoLast()
    {
        if (_history.Count == 0)
        {
            Console.WriteLine("No commands to undo.");
            return;
        }

        var cmd = _history.Pop();
        cmd.Undo();
    }

    public void UndoMultiple(int n)
    {
        for (int i = 0; i < n; i++)
        {
            if (_history.Count == 0) break;
            UndoLast();
        }
    }
}


abstract class Beverage
{
    public void PrepareRecipe()
    {
        BoilWater();
        Brew();
        PourInCup();
        if (CustomerWantsCondiments())
            AddCondiments();
    }

    protected void BoilWater() => Console.WriteLine("Boiling water");
    protected abstract void Brew();
    protected void PourInCup() => Console.WriteLine("Pouring into cup");
    protected abstract void AddCondiments();
    protected virtual bool CustomerWantsCondiments() => true;
}

class Tea : Beverage
{
    protected override void Brew() => Console.WriteLine("Steeping the tea");
    protected override void AddCondiments() => Console.WriteLine("Adding lemon");
}

class Coffee : Beverage
{
    protected override void Brew() => Console.WriteLine("Brewing the coffee");
    protected override void AddCondiments() => Console.WriteLine("Adding sugar and milk");
    protected override bool CustomerWantsCondiments()
    {
        Console.Write("Do you want condiments? (y/n): ");
        var ans = Console.ReadLine()?.Trim().ToLower();
        return ans == "y" || ans == "yes";
    }
}


interface IMediator
{
    void Register(User user);
    void Broadcast(string from, string message);
    void SendPrivate(string from, string to, string message);
    void Leave(User user);
}

class ChatRoom : IMediator
{
    private readonly Dictionary<string, User> _users = new Dictionary<string, User>(StringComparer.OrdinalIgnoreCase);

    public void Register(User user)
    {
        if (user == null || string.IsNullOrWhiteSpace(user.Name)) return;
        if (_users.ContainsKey(user.Name)) return;
        _users[user.Name] = user;
        user.SetMediator(this);
        Broadcast("System", $"{user.Name} joined the chat.");
    }

    public void Broadcast(string from, string message)
    {
        foreach (var u in _users.Values)
        {
            if (!string.Equals(u.Name, from, StringComparison.OrdinalIgnoreCase))
                u.Receive(from, message);
        }
    }

    public void SendPrivate(string from, string to, string message)
    {
        if (!_users.TryGetValue(to, out var recipient))
        {
            Console.WriteLine($"User '{to}' not found.");
            return;
        }
        recipient.Receive(from + " (private)", message);
    }

    public void Leave(User user)
    {
        if (user == null) return;
        if (_users.Remove(user.Name))
            Broadcast("System", $"{user.Name} left the chat.");
        user.SetMediator(null);
    }
}

class User
{
    public string Name { get; }
    private IMediator _mediator;
    public User(string name) => Name = name;

    public void SetMediator(IMediator mediator) => _mediator = mediator;

    public void Send(string message)
    {
        if (_mediator == null) { Console.WriteLine("You are not in a chat."); return; }
        Console.WriteLine($"{Name} (to all): {message}");
        _mediator.Broadcast(Name, message);
    }

    public void SendPrivate(string to, string message)
    {
        if (_mediator == null) { Console.WriteLine("You are not in a chat."); return; }
        Console.WriteLine($"{Name} -> {to}: {message}");
        _mediator.SendPrivate(Name, to, message);
    }

    public void Receive(string from, string message) => Console.WriteLine($"{Name} received from {from}: {message}");

    public void Leave()
    {
        _mediator?.Leave(this);
    }
}


class Program
{
    static void Main()
    {
        Console.WriteLine("Choose demo: 1-Command, 2-Template, 3-Mediator");
        var choice = Console.ReadLine()?.Trim();
        if (choice == "1") DemoCommand();
        else if (choice == "2") DemoTemplate();
        else if (choice == "3") DemoMediator();
        else Console.WriteLine("Exit.");
    }

    static void DemoCommand()
    {
        var light = new Light();
        var door = new Door();
        var thermo = new Thermostat();
        var inv = new Invoker();

        inv.ExecuteCommand(new LightOnCommand(light));
        inv.ExecuteCommand(new DoorOpenCommand(door));
        inv.ExecuteCommand(new IncreaseTempCommand(thermo, 3));

        Console.WriteLine("Undo last:");
        inv.UndoLast();

        inv.ExecuteCommand(new LightOffCommand(light));
        inv.ExecuteCommand(new DoorCloseCommand(door));

        Console.WriteLine("Undo two commands:");
        inv.UndoMultiple(2);

        Console.WriteLine("Attempt undo when history empty:");
        inv.UndoMultiple(10);
    }

    static void DemoTemplate()
    {
        Console.WriteLine("\n--- Making Tea ---");
        Beverage tea = new Tea();
        tea.PrepareRecipe();

        Console.WriteLine("\n--- Making Coffee ---");
        Beverage coffee = new Coffee();
        coffee.PrepareRecipe();
    }

    static void DemoMediator()
    {
        var chat = new ChatRoom();
        var alice = new User("Alice");
        var bob = new User("Bob");
        var carol = new User("Carol");

        chat.Register(alice);
        chat.Register(bob);
        chat.Register(carol);

        alice.Send("Hello everyone!");
        bob.SendPrivate("Carol", "Hi Carol, private message.");
        carol.Send("Thanks!");

        bob.Leave();

        alice.Send("Is Bob gone?");
    }
}
