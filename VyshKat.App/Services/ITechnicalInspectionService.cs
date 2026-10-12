using VyshKat.App.Domain;

namespace VyshKat.App.Services;

/// <summary>
/// Контракт сервиса технического осмотра.
/// </summary>
public interface ITechnicalInspectionService
{
    bool Inspect(Vehicle vehicle);
}