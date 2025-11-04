using System;
using System.Collections.Generic;


public interface IReport
{
    string Generate();
}

public class SalesReport : IReport
{
    public string Generate()
    {
        return "Sales Report: [item1:100$, item2:200$, item3:50$]";
    }
}
public class UserReport : IReport
{
    public string Generate()
    {
        return "User Report: [user1,user2,user3]";
    }
}

public abstract class ReportDecorator : IReport
{
    protected IReport report;
    public ReportDecorator(IReport report) { this.report = report; }
    public abstract string Generate();
}

public class DateFilterDecorator : ReportDecorator
{
    public DateFilterDecorator(IReport report) : base(report) { }
    public override string Generate()
    {
        return report.Generate() + " + Filtered by dates";
    }
}

public class SortingDecorator : ReportDecorator
{
    public SortingDecorator(IReport report) : base(report) { }
    public override string Generate()
    {
        return report.Generate() + " + Sorted";
    }
}

public class CsvExportDecorator : ReportDecorator
{
    public CsvExportDecorator(IReport report) : base(report) { }
    public override string Generate()
    {
        return report.Generate() + " + Exported to CSV";
    }
}

public class PdfExportDecorator : ReportDecorator
{
    public PdfExportDecorator(IReport report) : base(report) { }
    public override string Generate()
    {
        return report.Generate() + " + Exported to PDF";
    }
}


public interface IInternalDeliveryService
{
    void DeliverOrder(string orderId);
    string GetDeliveryStatus(string orderId);
}

public class InternalDeliveryService : IInternalDeliveryService
{
    public void DeliverOrder(string orderId)
    {
        Console.WriteLine($"[Internal] order {orderId} delivered");
    }

    public string GetDeliveryStatus(string orderId)
    {
        return "[Internal] delivered";
    }
}

public class ExternalLogisticsServiceA
{
    public void ShipItem(int itemId) { Console.WriteLine("[A] shipped " + itemId); }
    public string TrackShipment(int shipmentId) { return "[A] status OK"; }
}

public class LogisticsAdapterA : IInternalDeliveryService
{
    private ExternalLogisticsServiceA a = new ExternalLogisticsServiceA();
    public void DeliverOrder(string orderId)
    {
        a.ShipItem(orderId.GetHashCode());
    }
    public string GetDeliveryStatus(string orderId)
    {
        return a.TrackShipment(orderId.GetHashCode());
    }
}

public class ExternalLogisticsServiceB
{
    public void SendPackage(string packageInfo) { Console.WriteLine("[B] package " + packageInfo); }
    public string CheckPackageStatus(string trackingCode) { return "[B] in transit"; }
}

public class LogisticsAdapterB : IInternalDeliveryService
{
    private ExternalLogisticsServiceB b = new ExternalLogisticsServiceB();
    public void DeliverOrder(string orderId)
    {
        b.SendPackage(orderId);
    }
    public string GetDeliveryStatus(string orderId)
    {
        return b.CheckPackageStatus(orderId);
    }
}

public class DeliveryServiceFactory
{
    public static IInternalDeliveryService Get(string type)
    {
        if (type == "internal") return new InternalDeliveryService();
        if (type == "A") return new LogisticsAdapterA();
        if (type == "B") return new LogisticsAdapterB();
        throw new Exception("no such type");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== DECORATOR REPORT DEMO ===");

        IReport report = new SalesReport();
        report = new DateFilterDecorator(report);
        report = new SortingDecorator(report);
        report = new CsvExportDecorator(report);
        report = new PdfExportDecorator(report);

        Console.WriteLine(report.Generate());


        Console.WriteLine("\n=== ADAPTER LOGISTICS DEMO ===");

        IInternalDeliveryService delivery = DeliveryServiceFactory.Get("B");
        delivery.DeliverOrder("order#1001");
        Console.WriteLine(delivery.GetDeliveryStatus("order#1001"));
    }
}
