using System.Reflection;

Aquarium[] aquariums =
[
    new("Aqua Home", new FilterCartridge("FC-01", 420)),
    new("Coral Reef", new FilterCartridge("FC-02", 180)),
    new("Fresh Water", new FilterCartridge("FC-03", 360)),
];

Aquarium firstToNeedReplacement = AquariumUtils.LeastResource(aquariums);
Console.WriteLine($"Першим потребує заміни: {firstToNeedReplacement.Name}");

var interval = typeof(Aquarium).GetCustomAttribute<ServiceIntervalAttribute>();
const int daysSincePreviousService = 18;
int daysUntilService = interval!.Days - daysSincePreviousService;
Console.WriteLine($"До наступного обслуговування: {daysUntilService} діб");

class FilterCartridge(string id, double capacity)
{
    public string Id { get; } = id;

    public double Remaining { get; private set; } = capacity;
}

/// <summary>Містить операції для роботи з групою акваріумів.</summary>
static class AquariumUtils
{
    public static Aquarium LeastResource(Aquarium[] aquariums)
    {
        if (aquariums.Length == 0)
        {
            throw new ArgumentException("Масив акваріумів не може бути порожнім.", nameof(aquariums));
        }

        Aquarium least = aquariums[0];
        foreach (Aquarium aquarium in aquariums)
        {
            if (aquarium.Cartridge.Remaining < least.Cartridge.Remaining)
            {
                least = aquarium;
            }
        }

        return least;
    }
}

/// <summary>Задає періодичність обслуговування в добах.</summary>
[AttributeUsage(AttributeTargets.Class)]
class ServiceIntervalAttribute(int days) : Attribute
{
    public int Days { get; } = days;
}

[ServiceInterval(30)]
class Aquarium(string name, FilterCartridge cartridge)
{
    public string Name { get; } = name;

    public FilterCartridge Cartridge { get; } = cartridge;
}