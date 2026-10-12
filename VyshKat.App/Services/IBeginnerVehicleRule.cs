using VyshKat.App.Domain;

namespace VyshKat.App.Services;

public interface IBeginnerVehicleRule
{
    bool IsSuitable(Vehicle vehicle);
}
