using Moq;
using VyshKat.App.Services;
using VyshKat.App.Domain;

namespace VyshKat.Tests;

public class FleetManagerTests
{
    [Fact]
    public void AcceptVehicle_ShouldAddToInventory_WhenInspectionPasses()
    {
        var mockInspectionService = new Mock<ITechnicalInspectionService>(MockBehavior.Strict);
        mockInspectionService.Setup(s => s.Inspect(It.IsAny<Vehicle>())).Returns(true);
        var fleetManager = CreateFleetManager(mockInspectionService.Object);
        var scooter = new Scooter("S-ACCEPTED", 8, 2.0);

        var result = fleetManager.AcceptVehicle(scooter);

        Assert.True(result);
        Assert.Contains(scooter, fleetManager.GetAllInventoryItems());
        mockInspectionService.Verify(s => s.Inspect(scooter), Times.Once);
    }

    [Fact]
    public void AcceptVehicle_ShouldNotAddToInventory_WhenInspectionFails()
    {
        var mockInspectionService = new Mock<ITechnicalInspectionService>(MockBehavior.Strict);
        mockInspectionService.Setup(s => s.Inspect(It.IsAny<Vehicle>())).Returns(false);

        var fleetManager = CreateFleetManager(mockInspectionService.Object);
        var badScooter = new Scooter("BAD-1", 5, 2.0);

        bool result = fleetManager.AcceptVehicle(badScooter);
        Assert.False(result);
        Assert.Empty(fleetManager.GetAllInventoryItems());

        mockInspectionService.Verify(s => s.Inspect(badScooter), Times.Once);
    }

    [Fact]
    public void AcceptVehicle_ShouldRejectDuplicateInventoryNumber_WithoutRepeatingInspection()
    {
        var inspectionService = new Mock<ITechnicalInspectionService>(MockBehavior.Strict);
        inspectionService.Setup(s => s.Inspect(It.IsAny<Vehicle>())).Returns(true);
        var fleetManager = CreateFleetManager(inspectionService.Object);
        var firstVehicle = new Scooter("DUP-01", 8, 2.0);
        var duplicateVehicle = new Bicycle("dup-01", 8);

        var firstResult = fleetManager.AcceptVehicle(firstVehicle);
        var duplicateResult = fleetManager.AcceptVehicle(duplicateVehicle);

        Assert.True(firstResult);
        Assert.False(duplicateResult);
        Assert.Single(fleetManager.GetAllInventoryItems());
        inspectionService.Verify(s => s.Inspect(firstVehicle), Times.Once);
        inspectionService.Verify(s => s.Inspect(duplicateVehicle), Times.Never);
    }

    [Fact]
    public void AcceptThing_ShouldRejectNumberAlreadyUsedByVehicle()
    {
        var inspectionService = new Mock<ITechnicalInspectionService>();
        inspectionService.Setup(s => s.Inspect(It.IsAny<Vehicle>())).Returns(true);
        var fleetManager = CreateFleetManager(inspectionService.Object);
        fleetManager.AcceptVehicle(new Scooter("ITEM-01", 8, 2.0));

        var result = fleetManager.AcceptThing(new Helmet("item-01"));

        Assert.False(result);
        Assert.Single(fleetManager.GetAllInventoryItems());
    }

    [Fact]
    public void GetTotalEnergyConsumption_ShouldSumCorrectly_IgnoringNonConsumers()
    {
        var mockInspectionService = new Mock<ITechnicalInspectionService>();
        mockInspectionService.Setup(s => s.Inspect(It.IsAny<Vehicle>())).Returns(true);

        var fleetManager = CreateFleetManager(mockInspectionService.Object);

        fleetManager.AcceptVehicle(new Scooter("S-1", 5, 2.5));
        fleetManager.AcceptThing(new ChargingStation("CS-1", 10.0));

        fleetManager.AcceptVehicle(new Bicycle("B-1", 9));
        fleetManager.AcceptThing(new Helmet("H-1"));

        double totalEnergy = fleetManager.GetTotalEnergyConsumption();

        Assert.Equal(12.5, totalEnergy);
    }

    [Fact]
    public void GetBeginnerFriendlyVehicles_ShouldReturnOnlyVehiclesWithSimplicityAtLeastSix()
    {
        var mockInspectionService = new Mock<ITechnicalInspectionService>();
        mockInspectionService.Setup(s => s.Inspect(It.IsAny<Vehicle>())).Returns(true);
        var fleetManager = CreateFleetManager(mockInspectionService.Object);

        fleetManager.AcceptVehicle(new Scooter("S-6", 6, 1.0));
        fleetManager.AcceptVehicle(new Bicycle("B-5", 5));

        var beginnerVehicles = fleetManager.GetBeginnerFriendlyVehicles().ToList();

        Assert.Single(beginnerVehicles);
        Assert.Equal("S-6", beginnerVehicles[0].InventoryNumber);
    }

    [Fact]
    public void GetBeginnerFriendlyVehicles_ShouldUseInjectedRule()
    {
        var inspectionService = new Mock<ITechnicalInspectionService>();
        inspectionService.Setup(s => s.Inspect(It.IsAny<Vehicle>())).Returns(true);
        var beginnerRule = new Mock<IBeginnerVehicleRule>(MockBehavior.Strict);
        beginnerRule
            .Setup(rule => rule.IsSuitable(It.IsAny<Vehicle>()))
            .Returns((Vehicle vehicle) => vehicle.InventoryNumber == "S-ALLOWED");
        var fleetManager = new FleetManager(inspectionService.Object, beginnerRule.Object);
        var allowedVehicle = new Scooter("S-ALLOWED", 1, 1.0);
        var rejectedVehicle = new Scooter("S-REJECTED", 10, 1.0);
        fleetManager.AcceptVehicle(allowedVehicle);
        fleetManager.AcceptVehicle(rejectedVehicle);

        var beginnerVehicles = fleetManager.GetBeginnerFriendlyVehicles().ToList();

        Assert.Single(beginnerVehicles);
        Assert.Same(allowedVehicle, beginnerVehicles[0]);
        beginnerRule.Verify(rule => rule.IsSuitable(allowedVehicle), Times.Once);
        beginnerRule.Verify(rule => rule.IsSuitable(rejectedVehicle), Times.Once);
    }

    private static FleetManager CreateFleetManager(ITechnicalInspectionService inspectionService)
    {
        return new FleetManager(inspectionService, new BeginnerVehicleRule());
    }

    [Theory]
    [InlineData(5, false)]
    [InlineData(6, true)]
    [InlineData(9, true)]
    public void IsSuitableForBeginner_EvaluatesCorrectly_BasedOnSimplicity(int simplicity, bool expected)
    {
        var bicycle = new Bicycle("TEST-BIKE", simplicity);
        bool result = new BeginnerVehicleRule().IsSuitable(bicycle);
        Assert.Equal(expected, result);
    }
}

public class TechnicalInspectionServiceTests
{
    [Fact]
    public void Inspect_ShouldAcceptVehicle_WithRegularInventoryNumber()
    {
        var inspectionService = new TechnicalInspectionService();
        var scooter = new Scooter("SCOOT-OK", 8, 2.0);

        var result = inspectionService.Inspect(scooter);

        Assert.True(result);
    }

    [Fact]
    public void Inspect_ShouldRejectVehicle_WhenInventoryNumberContainsBrokenIgnoringCase()
    {
        var inspectionService = new TechnicalInspectionService();
        var scooter = new Scooter("scoot-bRoKeN-01", 8, 2.0);

        var result = inspectionService.Inspect(scooter);

        Assert.False(result);
    }
}