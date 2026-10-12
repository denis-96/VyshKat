using VyshKat.App.Domain;

namespace VyshKat.App.Services;

/// <summary>
/// Реализация сервиса технического осмотра.
/// </summary>
public class TechnicalInspectionService : ITechnicalInspectionService
{
    public bool Inspect(Vehicle vehicle)
    {
        // имитация логики осмотра
        // для демонстрационных целей бракуем технику, в номере которой есть слово "BROKEN"
        if (vehicle.InventoryNumber.Contains("BROKEN", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        return true;
    }
}