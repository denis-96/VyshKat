using VyshKat.App.Domain;

namespace VyshKat.App.Services;

/// <summary>
/// Контракт диспетчера парка.
/// </summary>
public interface IFleetManager
{
    bool AcceptVehicle(Vehicle vehicle);
    bool AcceptThing(Thing thing);
    double GetTotalEnergyConsumption();
    IEnumerable<Vehicle> GetBeginnerFriendlyVehicles();
    IEnumerable<IInventoryItem> GetAllInventoryItems();
}