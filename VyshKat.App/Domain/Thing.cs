namespace VyshKat.App.Domain;


/// <summary>
/// Базовая абстракция для недвижимого инвентаря (вещи).
/// </summary>
public abstract class Thing : IInventoryItem
{
    public string InventoryNumber { get; }
    public string Name { get; }

    protected Thing(string inventoryNumber, string name)
    {
        InventoryNumber = inventoryNumber;
        Name = name;
    }
}