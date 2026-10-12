namespace VyshKat.App.Domain;

/// <summary>
/// Контракт для устройств, потребляющих электричество.
/// </summary>
public interface IEnergyConsumer
{
    double DailyEnergyConsumption { get; }
}