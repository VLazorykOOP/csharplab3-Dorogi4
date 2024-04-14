using System;
using System.Linq;

public class Date
{
    private int day;
    private int month;
    private int year;

    public Date(int day, int month, int year)
    {
        Day = day;
        Month = month;
        Year = year;
    }

    public int Day
    {
        get { return day; }
        set
        {
            if (value < 1 || value > 31)
                throw new ArgumentOutOfRangeException(nameof(Day), "Day must be between 1 and 31.");
            day = value;
        }
    }

    public int Month
    {
        get { return month; }
        set
        {
            if (value < 1 || value > 12)
                throw new ArgumentOutOfRangeException(nameof(Month), "Month must be between 1 and 12.");
            month = value;
        }
    }

    public int Year
    {
        get { return year; }
        set { year = value; }
    }

    public string ToLongDateString()
    {
        return $"{day} {GetMonthName(month)} {year} року";
    }

    public string ToShortDateString()
    {
        return $"{day:D2}.{month:D2}.{year}";
    }

    private string GetMonthName(int month)
    {
        switch (month)
        {
            case 1: return "січня";
            case 2: return "лютого";
            case 3: return "березня";
            case 4: return "квітня";
            case 5: return "травня";
            case 6: return "червня";
            case 7: return "липня";
            case 8: return "серпня";
            case 9: return "вересня";
            case 10: return "жовтня";
            case 11: return "листопада";
            case 12: return "грудня";
            default: throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");
        }
    }

    public bool IsValidDate()
    {
        return DateTime.TryParse($"{day}/{month}/{year}", out _);
    }

    public static int DaysBetweenDates(Date date1, Date date2)
    {
        var dateTime1 = new DateTime(date1.Year, date1.Month, date1.Day);
        var dateTime2 = new DateTime(date2.Year, date2.Month, date2.Day);
        return Math.Abs((int)(dateTime2 - dateTime1).TotalDays);
    }

    public int Century
    {
        get { return (year - 1) / 100 + 1; }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Виберіть завдання для виконання:");
        Console.WriteLine("1. Опрацювання дат");
        Console.WriteLine("2. Робота з документами");
        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            ProcessDates();
        }
        else if (choice == 2)
        {
            ProcessDocuments();
        }
        else
        {
            Console.WriteLine("Невірний вибір завдання.");
        }
    }

    static void ProcessDates()
    {
        Console.WriteLine("Введіть кількість дат:");
        int count = int.Parse(Console.ReadLine());

        Date[] dates = new Date[count];
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"Введіть дату {i + 1} (день місяць рік через пробіл):");
            string[] parts = Console.ReadLine().Split(' ');
            int day = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int year = int.Parse(parts[2]);
            dates[i] = new Date(day, month, year);
        }

        // Виведення дат у впорядкованому порядку за зростанням
        var sortedDates = dates.OrderBy(d => new DateTime(d.Year, d.Month, d.Day));
        Console.WriteLine("Дати впорядковані за зростанням:");
        foreach (var date in sortedDates)
        {
            Console.WriteLine(date.ToShortDateString());
        }

        // Знаходження найбільшої кількості днів між датами
        int maxDays = 0;
        for (int i = 0; i < dates.Length - 1; i++)
        {
            for (int j = i + 1; j < dates.Length; j++)
            {
                int days = Date.DaysBetweenDates(dates[i], dates[j]);
                if (days > maxDays)
                    maxDays = days;
            }
        }
        Console.WriteLine($"Найбільша кількість днів між датами: {maxDays}");
    }

    static void ProcessDocuments()
    {
        // Створення масиву базового класу
        Document[] documents = new Document[4];

        // Функція, яка наповнює масив різними об'єктами похідних класів
        FillArray(documents);

        // Виведення масиву впорядкованого за деяким критерієм
        Array.Sort(documents, (x, y) => x.Date.CompareTo(y.Date));
        Console.WriteLine("Sorted Array:");
        foreach (var document in documents)
        {
            document.Show();
            Console.WriteLine();
        }
    }

    // Функція для наповнення масиву об'єктами різних класів
    static void FillArray(Document[] documents)
    {
        documents[0] = new Invoice("Invoice 1", DateTime.Now, 100.50m);
        documents[1] = new Receipt("Receipt 1", DateTime.Now.AddDays(-1), "Company A");
        documents[2] = new Invoice("Invoice 2", DateTime.Now.AddDays(-2), 200.75m);
        documents[3] = new Receipt("Receipt 2", DateTime.Now.AddDays(-3), "Company B");
    }
}

// Базовий клас
class Document
{
    public string Title { get; set; }
    public DateTime Date { get; set; }

    // Конструктор
    public Document(string title, DateTime date)
    {
        Title = title;
        Date = date;
    }

    // Метод для виведення даних про об'єкт класу
    public virtual void Show()
    {
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Date: {Date}");
    }
}

// Похідний клас 1
class Invoice : Document
{
    public decimal Amount { get; set; }

    // Конструктор
    public Invoice(string title, DateTime date, decimal amount)
        : base(title, date)
    {
        Amount = amount;
    }

    // Перевизначений метод для виведення даних про об'єкт класу
    public override void Show()
    {
        base.Show();
        Console.WriteLine($"Amount: {Amount}");
    }
}

// Похідний клас 2
class Receipt : Document
{
    public string Issuer { get; set; }

    // Конструктор
    public Receipt(string title, DateTime date, string issuer)
        : base(title, date)
    {
        Issuer = issuer;
    }

    // Перевизначений метод для виведення даних про об'єкт класу
    public override void Show()
    {
        base.Show();
        Console.WriteLine($"Issuer: {Issuer}");
    }
}
