namespace VyshKat.App.Domain;

/// <summary>
/// Механический велосипед. Транспорт, но не потребляет энергию.
/// </summary>
public class Bicycle : Vehicle
{
    public Bicycle(string inventoryNumber, int simplicity)
        : base(inventoryNumber, simplicity)
    {
    }
}