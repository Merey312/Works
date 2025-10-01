using System;

public interface IDocument
{
    void Open();
}

public class Report : IDocument
{
    public void Open() => Console.WriteLine("📊 Открыт отчет.");
}

public class Resume : IDocument
{
    public void Open() => Console.WriteLine("📄 Открыто резюме.");
}

public class Letter : IDocument
{
    public void Open() => Console.WriteLine("✉ Открыто письмо.");
}

public class Invoice : IDocument
{
    public void Open() => Console.WriteLine("🧾 Открыт счет-фактура.");
}

public abstract class DocumentCreator
{
    public abstract IDocument CreateDocument();
}

public class ReportCreator : DocumentCreator
{
    public override IDocument CreateDocument() => new Report();
}

public class ResumeCreator : DocumentCreator
{
    public override IDocument CreateDocument() => new Resume();
}

public class LetterCreator : DocumentCreator
{
    public override IDocument CreateDocument() => new Letter();
}

public class InvoiceCreator : DocumentCreator
{
    public override IDocument CreateDocument() => new Invoice();
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Выберите документ: 1 - Отчет, 2 - Резюме, 3 - Письмо, 4 - Счет-фактура");
        string choice = Console.ReadLine();

        DocumentCreator creator = choice switch
        {
            "1" => new ReportCreator(),
            "2" => new ResumeCreator(),
            "3" => new LetterCreator(),
            "4" => new InvoiceCreator(),
            _ => null
        };

        if (creator == null)
        {
            Console.WriteLine("Ошибка: неверный выбgр документа!");
            return;
        }

        IDocument doc = creator.CreateDocument();
        doc.Open();
    }
}
