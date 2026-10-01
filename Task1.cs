var aquarium = new Aquarium("Aqua Home", new FilterCartridge("FC-01", 750));

bool firstFiltering = aquarium.Filter(3);
Console.WriteLine($"Фільтрація 3 діб: {firstFiltering}, залишок: {aquarium.Cartridge.Remaining} л");

bool secondFiltering = aquarium.Filter(4);
Console.WriteLine($"Фільтрація 4 діб: {secondFiltering}, залишок: {aquarium.Cartridge.Remaining} л");

aquarium.ReplaceCartridge(new FilterCartridge("FC-02", 900));
Console.WriteLine($"Новий картридж {aquarium.Cartridge.Id}: {aquarium.Cartridge.Remaining} л");

/// <summary>Фільтр-картридж з обліком залишкового ресурсу в літрах.</summary>
class FilterCartridge(string id, double capacity)
{
    public string Id { get; } = id;

    public double Remaining { get; private set; } = capacity;

    /// <summary>Витрачає ресурс, лише якщо його достатньо.</summary>
    public bool TryConsume(double liters)
    {
        if (liters > Remaining)
        {
            return false;
        }

        Remaining -= liters;
        return true;
    }
}

/// <summary>Акваріум, що володіє фільтр-картриджем.</summary>
class Aquarium(string name, FilterCartridge cartridge)
{
    public string Name { get; } = name;

    public FilterCartridge Cartridge { get; private set; } = cartridge;

    /// <summary>Фільтрація витрачає 150 л ресурсу за добу.</summary>
    public bool Filter(int days) => Cartridge.TryConsume(days * 150);

    public void ReplaceCartridge(FilterCartridge newCartridge)
    {
        Cartridge = newCartridge;
    }
}