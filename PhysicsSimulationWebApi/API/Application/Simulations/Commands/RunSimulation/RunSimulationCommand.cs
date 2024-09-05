namespace PhysicsSimulationWebApi.Application.Simulations.Commands.RunSimulation;

public sealed record RunSimulationCommand
{
    public decimal AirTemperature { get; private set; }
    public decimal AirVelocity { get; private set; }
    public decimal SolarPanelLength { get; private set; }
    public decimal SolarPanelSurfaceTemperature { get; private set; }
    public decimal SolarIrradiation { get; private set; }
    public decimal FilmTemperature => (AirTemperature + SolarPanelSurfaceTemperature) / 2.0M;
    public decimal StorageTankHeight { get; private set; }
    public decimal StorageTankRadius { get; private set; }
    public decimal InitialTankFluidTemperature { get; private set; }
    public decimal TankFluidMassFlowRate { get; private set; }

    public RunSimulationCommand(
        decimal airTemperature, 
        decimal airVelocity, 
        decimal solarPanelLength, 
        decimal solarPanelSurfaceTemperature, 
        decimal solarIrradiation, 
        decimal storageTankHeight, 
        decimal storageTankRadius, 
        decimal initialTankFluidTemperature,
        decimal tankFluidMassFlowRate)
    {
        AirTemperature = airTemperature;
        AirVelocity = airVelocity;
        SolarPanelLength = solarPanelLength;
        SolarPanelSurfaceTemperature = solarPanelSurfaceTemperature;
        SolarIrradiation = solarIrradiation;
        StorageTankHeight = storageTankHeight;
        StorageTankRadius = storageTankRadius;
        InitialTankFluidTemperature = initialTankFluidTemperature;
        TankFluidMassFlowRate = tankFluidMassFlowRate;
    }
}