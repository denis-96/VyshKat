using VyshKat.App.Domain;

namespace VyshKat.App.Services;

/// <summary>
/// Реализация диспетчера парка.
/// Сохраняет состояние.
/// </summary>
public class FleetManager : IFleetManager
{
    private readonly List<IInventoryItem> _inventory;
    private readonly ITechnicalInspectionService _inspectionService;
    private readonly IBeginnerVehicleRule _beginnerVehicleRule;

    // внедрение зависимости
    public FleetManager(
        ITechnicalInspectionService inspectionService,
        IBeginnerVehicleRule beginnerVehicleRule)
    {
        _inventory = new List<IInventoryItem>();
        _inspectionService = inspectionService;
        _beginnerVehicleRule = beginnerVehicleRule;
    }

    public bool AcceptVehicle(Vehicle vehicle)
    {
        if (HasInventoryNumber(vehicle.InventoryNumber))
        {
            return false;
        }

        // перед приемкой обязателен техосмотр
        if (_inspectionService.Inspect(vehicle))
        {
            _inventory.Add(vehicle);
            return true;
        }
        return false; // отказ в приемке
    }

    public bool AcceptThing(Thing thing)
    {
        if (HasInventoryNumber(thing.InventoryNumber))
        {
            return false;
        }

        _inventory.Add(thing);
        return true;
    }

    public double GetTotalEnergyConsumption()
    {
        // выбираем только потребителей энергии и суммируем
        return _inventory
            .OfType<IEnergyConsumer>()
            .Sum(item => item.DailyEnergyConsumption);
    }

    public IEnumerable<Vehicle> GetBeginnerFriendlyVehicles()
    {
        // фильтрация транспорта через заменяемое бизнес-правило
        return _inventory
            .OfType<Vehicle>()
            .Where(_beginnerVehicleRule.IsSuitable);
    }

    public IEnumerable<IInventoryItem> GetAllInventoryItems()
    {
        return _inventory.AsReadOnly();
    }

    private bool HasInventoryNumber(string inventoryNumber)
    {
        return _inventory.Any(item => string.Equals(
            item.InventoryNumber,
            inventoryNumber,
            StringComparison.OrdinalIgnoreCase));
    }
}