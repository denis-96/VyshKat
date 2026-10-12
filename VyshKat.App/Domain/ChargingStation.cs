namespace VyshKat.App.Domain;

/// <summary>
/// Зарядная станция. Вещь, потребляющая электричество.
/// </summary>
public class ChargingStation : Thing, IEnergyConsumer
{
    public double DailyEnergyConsumption { get; }

    public ChargingStation(string inventoryNumber, double dailyEnergyConsumption)
        : base(inventoryNumber, "Зарядная станция")
    {
        DailyEnergyConsumption = dailyEnergyConsumption;
    }
}