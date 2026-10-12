namespace VyshKat.App.Domain;

/// <summary>
/// Шлем. Вещь, не потребляющая энергию.
/// </summary>
public class Helmet : Thing
{
    public Helmet(string inventoryNumber)
        : base(inventoryNumber, "Защитный шлем")
    {
    }
}