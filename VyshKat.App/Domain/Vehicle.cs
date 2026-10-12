namespace VyshKat.App.Domain;

/// <summary>
/// Базовая абстракция для всех транспортных средств.
/// </summary>
public abstract class Vehicle : IInventoryItem
{
    public string InventoryNumber { get; }

    /// <summary>
    /// Уровень простоты управления от 1 до 10.
    /// </summary>
    public int Simplicity { get; }

    protected Vehicle(string inventoryNumber, int simplicity)
    {
        if (simplicity < 1 || simplicity > 10)
            throw new ArgumentOutOfRangeException(nameof(simplicity), "Простота должна быть от 1 до 10");

        InventoryNumber = inventoryNumber;
        Simplicity = simplicity;
    }
}