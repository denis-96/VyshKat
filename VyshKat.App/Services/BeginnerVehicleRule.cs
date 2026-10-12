using VyshKat.App.Domain;

namespace VyshKat.App.Services;

public class BeginnerVehicleRule : IBeginnerVehicleRule
{
    public bool IsSuitable(Vehicle vehicle)
    {
        return vehicle.Simplicity >= 6;
    }
}
