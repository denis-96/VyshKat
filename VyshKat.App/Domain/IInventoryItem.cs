namespace VyshKat.App.Domain;

/// <summary>
/// Базовый контракт для всех объектов на балансе университета.
/// Гарантирует наличие инвентарного номера.
/// </summary>
public interface IInventoryItem
{
    string InventoryNumber { get; }
}