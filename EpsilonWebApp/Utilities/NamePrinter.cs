namespace EpsilonWebApp.Utilities;

public class Employee : INameable
{
    public string Name { get; set; } = string.Empty;
}

public class Manager : INameable
{
    public string Name { get; set; } = string.Empty;
}

public interface INameable
{
    string Name { get; }
}

public class NamePrinter
{
    public void PrintName<T>(T entity) where T : INameable
    {
        Console.WriteLine(entity.Name);
    }
}