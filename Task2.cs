var aquarium = new Aquarium("Aqua Home", new FilterCartridge("FC-01", 750));

aquarium.Filter(3); // 450 l are consumed; 300 l remain.
aquarium.ReplaceCartridge(new FilterCartridge("FC-02", 900), new DateOnly(2026, 9, 25));
aquarium.Filter(4); // 600 l are consumed; 300 l remain.
aquarium.ReplaceCartridge(new FilterCartridge("FC-03", 750), new DateOnly(2026, 10, 3));

aquarium.PrintHistory();

class FilterCartridge(string id, double capacity)
{
    public string Id { get; } = id;

    public double Remaining { get; private set; } = capacity;

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

class Aquarium(string name, FilterCartridge cartridge)
{
    // This field has no public access; the nested record reads it via Aquarium.
    private int completedOperations;
    private readonly List<ServiceRecord> history = [];

    public string Name { get; } = name;

    public FilterCartridge Cartridge { get; private set; } = cartridge;

    public bool Filter(int days)
    {
        if (!Cartridge.TryConsume(days * 150))
        {
            return false;
        }

        completedOperations++;
        return true;
    }

    public void ReplaceCartridge(FilterCartridge newCartridge, DateOnly date)
    {
        history.Add(new ServiceRecord(this, date));
        Cartridge = newCartridge;
    }

    public void PrintHistory()
    {
        foreach (ServiceRecord record in history)
        {
            Console.WriteLine(
                $"{record.Date:dd.MM.yyyy} | картридж {record.CartridgeId} | " +
                $"залишок {record.ResourceLeft} л | операцій {record.CompletedOperations}");
        }
    }

    /// <summary>Незмінний запис про картридж, який було замінено.</summary>
    public record ServiceRecord(
        DateOnly Date,
        string CartridgeId,
        double ResourceLeft,
        int CompletedOperations)
    {
        public ServiceRecord(Aquarium aquarium, DateOnly date)
            : this(
                date,
                aquarium.Cartridge.Id,
                aquarium.Cartridge.Remaining,
                aquarium.completedOperations)
        {
        }
    }
}