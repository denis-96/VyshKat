namespace VyshKat.App.Domain;

/// <summary>
/// Электросамокат. Является транспортом и потребляет энергию.
/// </summary>
public class Scooter : Vehicle, IEnergyConsumer
{
    public double DailyEnergyConsumption { get; }

    public Scooter(string inventoryNumber, int simplicity, double dailyEnergyConsumption)
        : base(inventoryNumber, simplicity)
    {
        DailyEnergyConsumption = dailyEnergyConsumption;
    }
}