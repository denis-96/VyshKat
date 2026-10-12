using Microsoft.Extensions.DependencyInjection;
using VyshKat.App.Services;
using VyshKat.App.UI;

internal static class Program
{
    public static void Main()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITechnicalInspectionService, TechnicalInspectionService>();
        services.AddTransient<IBeginnerVehicleRule, BeginnerVehicleRule>();
        services.AddSingleton<IFleetManager, FleetManager>();
        services.AddTransient<ConsoleUserInterface>();

        using var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        });

        serviceProvider.GetRequiredService<ConsoleUserInterface>().Run();
    }
}