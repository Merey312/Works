using System;
using System.Collections.Generic;

interface ICommand
{
    void Execute();
    void Undo();
    string CommandName { get; }
}

class Logger
{
    private readonly List<string> _lines = new List<string>();
    public void Log(string text)
    {
        var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {text}";
        _lines.Add(line);
        Console.WriteLine(line);
    }
    public IEnumerable<string> GetAll() => _lines;
}

class Light
{
    public string Name { get; }
    public bool IsOn { get; private set; }
    public Light(string name) => Name = name;
    public void On()
    {
        IsOn = true;
        Console.WriteLine($"Свет \"{Name}\": включён");
    }
    public void Off()
    {
        IsOn = false;
        Console.WriteLine($"Свет \"{Name}\": выключен");
    }
}

class Television
{
    public string Name { get; }
    public bool IsOn { get; private set; }
    public Television(string name) => Name = name;
    public void On()
    {
        IsOn = true;
        Console.WriteLine($"Телевизор \"{Name}\": включён");
    }
    public void Off()
    {
        IsOn = false;
        Console.WriteLine($"Телевизор \"{Name}\": выключен");
    }
}

class AirConditioner
{
    public string Name { get; }
    public bool IsOn { get; private set; }
    public int Temperature { get; private set; } = 24;
    public string Mode { get; private set; } = "Обычный";
    public AirConditioner(string name) => Name = name;
    public void On()
    {
        IsOn = true;
        Console.WriteLine($"Кондиционер \"{Name}\": включён");
    }
    public void Off()
    {
        IsOn = false;
        Console.WriteLine($"Кондиционер \"{Name}\": выключен");
    }
    public void SetTemperature(int temp)
    {
        Temperature = temp;
        Console.WriteLine($"Кондиционер \"{Name}\": температура установлена {Temperature}°C");
    }
    public void SetMode(string mode)
    {
        Mode = mode;
        Console.WriteLine($"Кондиционер \"{Name}\": режим \"{Mode}\"");
    }
}

class LightOnCommand : ICommand
{
    private readonly Light _light;
    private readonly Logger _logger;
    public string CommandName => $"Включить свет {_light.Name}";
    public LightOnCommand(Light light, Logger logger) { _light = light; _logger = logger; }
    public void Execute() { _light.On(); _logger.Log(CommandName); }
    public void Undo() { _light.Off(); _logger.Log($"Отмена: {CommandName}"); }
}

class LightOffCommand : ICommand
{
    private readonly Light _light;
    private readonly Logger _logger;
    public string CommandName => $"Выключить свет {_light.Name}";
    public LightOffCommand(Light light, Logger logger) { _light = light; _logger = logger; }
    public void Execute() { _light.Off(); _logger.Log(CommandName); }
    public void Undo() { _light.On(); _logger.Log($"Отмена: {CommandName}"); }
}

class TvOnCommand : ICommand
{
    private readonly Television _tv;
    private readonly Logger _logger;
    public string CommandName => $"Включить ТВ {_tv.Name}";
    public TvOnCommand(Television tv, Logger logger) { _tv = tv; _logger = logger; }
    public void Execute() { _tv.On(); _logger.Log(CommandName); }
    public void Undo() { _tv.Off(); _logger.Log($"Отмена: {CommandName}"); }
}

class TvOffCommand : ICommand
{
    private readonly Television _tv;
    private readonly Logger _logger;
    public string CommandName => $"Выключить ТВ {_tv.Name}";
    public TvOffCommand(Television tv, Logger logger) { _tv = tv; _logger = logger; }
    public void Execute() { _tv.Off(); _logger.Log(CommandName); }
    public void Undo() { _tv.On(); _logger.Log($"Отмена: {CommandName}"); }
}

class AcOnCommand : ICommand
{
    private readonly AirConditioner _ac;
    private readonly Logger _logger;
    public string CommandName => $"Включить AC {_ac.Name}";
    public AcOnCommand(AirConditioner ac, Logger logger) { _ac = ac; _logger = logger; }
    public void Execute() { _ac.On(); _logger.Log(CommandName); }
    public void Undo() { _ac.Off(); _logger.Log($"Отмена: {CommandName}"); }
}

class AcOffCommand : ICommand
{
    private readonly AirConditioner _ac;
    private readonly Logger _logger;
    public string CommandName => $"Выключить AC {_ac.Name}";
    public AcOffCommand(AirConditioner ac, Logger logger) { _ac = ac; _logger = logger; }
    public void Execute() { _ac.Off(); _logger.Log(CommandName); }
    public void Undo() { _ac.On(); _logger.Log($"Отмена: {CommandName}"); }
}

class SetAcTemperatureCommand : ICommand
{
    private readonly AirConditioner _ac;
    private readonly int _temp;
    private readonly Logger _logger;
    private int _previous;
    public string CommandName => $"Установить AC {_ac.Name} Темп {_temp}";
    public SetAcTemperatureCommand(AirConditioner ac, int temp, Logger logger) { _ac = ac; _temp = temp; _logger = logger; }
    public void Execute() { _previous = _ac.Temperature; _ac.SetTemperature(_temp); _logger.Log(CommandName); }
    public void Undo() { _ac.SetTemperature(_previous); _logger.Log($"Отмена: {CommandName}"); }
}

class SetAcModeCommand : ICommand
{
    private readonly AirConditioner _ac;
    private readonly string _mode;
    private readonly Logger _logger;
    private string _previous;
    public string CommandName => $"Режим AC {_ac.Name} -> {_mode}";
    public SetAcModeCommand(AirConditioner ac, string mode, Logger logger) { _ac = ac; _mode = mode; _logger = logger; }
    public void Execute() { _previous = _ac.Mode; _ac.SetMode(_mode); _logger.Log(CommandName); }
    public void Undo() { _ac.SetMode(_previous); _logger.Log($"Отмена: {CommandName}"); }
}

class MacroCommand : ICommand
{
    private readonly List<ICommand> _commands;
    private readonly Logger _logger;
    public string CommandName { get; }
    public MacroCommand(IEnumerable<ICommand> commands, string name, Logger logger)
    {
        _commands = new List<ICommand>(commands);
        CommandName = $"Macro: {name}";
        _logger = logger;
    }
    public void Execute()
    {
        _logger.Log($"Выполнение макрокоманды: {CommandName}");
        foreach (var cmd in _commands) cmd.Execute();
    }
    public void Undo()
    {
        _logger.Log($"Отмена макрокоманды: {CommandName}");
        for (int i = _commands.Count - 1; i >= 0; i--) _commands[i].Undo();
    }
}

class RemoteControl
{
    private readonly Dictionary<int, (ICommand on, ICommand off)> _slots = new();
    private readonly Stack<ICommand> _history = new();
    private readonly Logger _logger;
    public RemoteControl(Logger logger) { _logger = logger; }
    public void SetCommand(int slot, ICommand onCommand, ICommand offCommand)
    {
        _slots[slot] = (onCommand, offCommand);
        _logger.Log($"Назначены команды в слот {slot}");
    }
    public void PressOn(int slot)
    {
        if (!_slots.TryGetValue(slot, out var pair) || pair.on == null)
        {
            _logger.Log($"Ошибка: в слоте {slot} нет команды 'включить'");
            Console.WriteLine($"Ошибка: в слоте {slot} не назначена команда 'включить'");
            return;
        }
        pair.on.Execute();
        _history.Push(pair.on);
    }
    public void PressOff(int slot)
    {
        if (!_slots.TryGetValue(slot, out var pair) || pair.off == null)
        {
            _logger.Log($"Ошибка: в слоте {slot} нет команды 'выключить'");
            Console.WriteLine($"Ошибка: в слоте {slot} не назначена команда 'выключить'");
            return;
        }
        pair.off.Execute();
        _history.Push(pair.off);
    }
    public void PressUndo()
    {
        if (_history.Count == 0)
        {
            _logger.Log("Попытка отмены, но история пуста");
            Console.WriteLine("Нечего отменять");
            return;
        }
        var cmd = _history.Pop();
        cmd.Undo();
    }
    public void PressUndoMultiple(int n)
    {
        for (int i = 0; i < n; i++) PressUndo();
    }
    public void ShowSlots()
    {
        Console.WriteLine("Текущие слоты:");
        foreach (var kv in _slots)
        {
            string on = kv.Value.on?.CommandName ?? "(пусто)";
            string off = kv.Value.off?.CommandName ?? "(пусто)";
            Console.WriteLine($"Слот {kv.Key}: ON -> {on}, OFF -> {off}");
        }
    }
}

class Program
{
    static void Main()
    {
        var logger = new Logger();

        var livingLight = new Light("Гостиная");
        var kitchenLight = new Light("Кухня");
        var tv = new Television("LG");
        var ac = new AirConditioner("Samsung");

        var remote = new RemoteControl(logger);

        remote.SetCommand(1, new LightOnCommand(livingLight, logger), new LightOffCommand(livingLight, logger));
        remote.SetCommand(2, new LightOnCommand(kitchenLight, logger), new LightOffCommand(kitchenLight, logger));
        remote.SetCommand(3, new TvOnCommand(tv, logger), new TvOffCommand(tv, logger));
        remote.SetCommand(4, new AcOnCommand(ac, logger), new AcOffCommand(ac, logger));
        remote.SetCommand(5, new SetAcTemperatureCommand(ac, 20, logger), null);

        remote.ShowSlots();

        Console.WriteLine("\nУправление с пульта:");
        remote.PressOn(1);
        remote.PressOff(1);
        remote.PressOn(3);
        remote.PressOn(4);
        remote.PressOn(5);

        Console.WriteLine("\nСоздаём макрокоманду: 'Утро' (включить свет, включить телевизор, установить AC)");
        var morningMacro = new MacroCommand(new ICommand[]
        {
            new LightOnCommand(livingLight, logger),
            new LightOnCommand(kitchenLight, logger),
            new TvOnCommand(tv, logger),
            new SetAcTemperatureCommand(ac, 22, logger),
            new AcOnCommand(ac, logger)
        }, "Утро", logger);

        remote.SetCommand(9, morningMacro, null);
        remote.PressOn(9);

        Console.WriteLine("\nОтмена последней команды (макрокоманды):");
        remote.PressUndo();

        Console.WriteLine("\nОтмена двух команд:");
        remote.PressUndoMultiple(2);

        Console.WriteLine("\nПопытка нажать пустой слот 7:");
        remote.PressOn(7);

        Console.WriteLine("\nИстория логов:");
        foreach (var line in logger.GetAll()) { /* вывод уже делается в Log, можно здесь ничего не делать */ }

        Console.WriteLine("\nГотово.");
    }
}

