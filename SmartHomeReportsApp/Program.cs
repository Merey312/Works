using System;
using System.Collections.Generic;

interface ICommand { void Execute(); void Undo(); string Name { get; } }

class Light
{
    public string Name { get; }
    public Light(string name) => Name = name;
    public void On() => Console.WriteLine($"Свет {Name} включён");
    public void Off() => Console.WriteLine($"Свет {Name} выключен");
}

class Television
{
    public string Name { get; }
    public Television(string name) => Name = name;
    public void On() => Console.WriteLine($"ТВ {Name} включён");
    public void Off() => Console.WriteLine($"ТВ {Name} выключен");
}

class AirConditioner
{
    public string Name { get; }
    public int Temperature { get; private set; } = 24;
    public AirConditioner(string name) => Name = name;
    public void On() => Console.WriteLine($"Кондиционер {Name} включён");
    public void Off() => Console.WriteLine($"Кондиционер {Name} выключен");
    public void SetTemp(int t) { Temperature = t; Console.WriteLine($"Температура {Name}: {t}°C"); }
}

class LightOn : ICommand
{
    Light l; public string Name => $"Включить {l.Name}";
    public LightOn(Light x) => l = x;
    public void Execute() => l.On();
    public void Undo() => l.Off();
}
class LightOff : ICommand
{
    Light l; public string Name => $"Выключить {l.Name}";
    public LightOff(Light x) => l = x;
    public void Execute() => l.Off();
    public void Undo() => l.On();
}
class TvOn : ICommand
{
    Television t; public string Name => $"Включить {t.Name}";
    public TvOn(Television x) => t = x;
    public void Execute() => t.On();
    public void Undo() => t.Off();
}
class TvOff : ICommand
{
    Television t; public string Name => $"Выключить {t.Name}";
    public TvOff(Television x) => t = x;
    public void Execute() => t.Off();
    public void Undo() => t.On();
}
class AcOn : ICommand
{
    AirConditioner a; public string Name => $"Включить {a.Name}";
    public AcOn(AirConditioner x) => a = x;
    public void Execute() => a.On();
    public void Undo() => a.Off();
}
class AcOff : ICommand
{
    AirConditioner a; public string Name => $"Выключить {a.Name}";
    public AcOff(AirConditioner x) => a = x;
    public void Execute() => a.Off();
    public void Undo() => a.On();
}
class AcTemp : ICommand
{
    AirConditioner a; int t, prev;
    public string Name => $"Температура {a.Name} = {t}";
    public AcTemp(AirConditioner x, int temp) { a = x; t = temp; }
    public void Execute() { prev = a.Temperature; a.SetTemp(t); }
    public void Undo() => a.SetTemp(prev);
}
class Macro : ICommand
{
    List<ICommand> cmds; public string Name => "Макрокоманда";
    public Macro(params ICommand[] c) => cmds = new List<ICommand>(c);
    public void Execute() { foreach (var x in cmds) x.Execute(); }
    public void Undo() { for (int i = cmds.Count - 1; i >= 0; i--) cmds[i].Undo(); }
}
class Remote
{
    Dictionary<int, ICommand> slots = new();
    Stack<ICommand> undo = new(), redo = new();
    public void Set(int s, ICommand c) => slots[s] = c;
    public void Press(int s)
    {
        if (!slots.TryGetValue(s, out var c) || c == null) { Console.WriteLine($"Слот {s} пуст"); return; }
        c.Execute(); undo.Push(c); redo.Clear();
    }
    public void Undo()
    {
        if (undo.Count == 0) { Console.WriteLine("Нечего отменять"); return; }
        var c = undo.Pop(); c.Undo(); redo.Push(c);
    }
    public void Redo()
    {
        if (redo.Count == 0) { Console.WriteLine("Нечего повторять"); return; }
        var c = redo.Pop(); c.Execute(); undo.Push(c);
    }
}

abstract class Report
{
    public void Generate() { Load(); Format(); if (Ask("Сохранить?")) Save(); if (Ask("Отправить?")) Send(); }
    protected abstract void Load(); protected abstract void Format(); protected abstract void Save(); protected virtual void Send() { }
    bool Ask(string q) { Console.Write(q + " (y/n): "); var s = Console.ReadLine()?.Trim().ToLower(); return s == "y"; }
}
class Pdf : Report
{
    protected override void Load() => Console.WriteLine("PDF: загрузка");
    protected override void Format() => Console.WriteLine("PDF: формат");
    protected override void Save() => Console.WriteLine("PDF: сохранён");
    protected override void Send() => Console.WriteLine("PDF: отправлен");
}
class Excel : Report
{
    protected override void Load() => Console.WriteLine("Excel: загрузка");
    protected override void Format() => Console.WriteLine("Excel: формат");
    protected override void Save() => Console.WriteLine("Excel: сохранён");
}
class Html : Report
{
    protected override void Load() => Console.WriteLine("HTML: загрузка");
    protected override void Format() => Console.WriteLine("HTML: формат");
    protected override void Save() => Console.WriteLine("HTML: сохранён");
}

class Program
{
    static void Main()
    {
        Console.WriteLine("1 - Умный дом, 2 - Отчёты");
        var m = Console.ReadLine();
        if (m == "1") Home(); else if (m == "2") Reports();
    }
    static void Home()
    {
        var l = new Light("Гостиная");
        var tv = new Television("LG");
        var ac = new AirConditioner("Samsung");
        var r = new Remote();
        r.Set(1, new LightOn(l));
        r.Set(2, new LightOff(l));
        r.Set(3, new TvOn(tv));
        r.Set(4, new TvOff(tv));
        r.Set(5, new AcOn(ac));
        r.Set(6, new AcOff(ac));
        r.Set(7, new AcTemp(ac, 20));
        r.Set(8, new Macro(new LightOn(l), new TvOn(tv), new AcOn(ac)));
        r.Press(1);
        r.Press(3);
        r.Press(5);
        r.Undo();
        r.Redo();
        r.Press(8);
    }
    static void Reports()
    {
        Console.WriteLine("1 - PDF, 2 - Excel, 3 - HTML");
        var c = Console.ReadLine();
        Report r = c switch { "1" => new Pdf(), "2" => new Excel(), "3" => new Html(), _ => null };
        r?.Generate();
    }
}

