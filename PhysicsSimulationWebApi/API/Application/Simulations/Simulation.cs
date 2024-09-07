using PassiveLogicPhysicsSimulation.Empirical;
using PhysicsSimulationWebApi.Application.Simulations.Commands.RunSimulation;
using PhysicsSimulationWebApi.Application.Simulations.SystemComponents;
using PhysicsSimulationWebApi.Application.Simulations.SystemComponents.Air;
using PhysicsSimulationWebApi.Application.Simulations.SystemComponents.Water;
using PhysicsSimulationWebApi.Application.Simulations.Theoretical;
using Environment = PhysicsSimulationWebApi.Application.Simulations.SystemComponents.Environment;

namespace PhysicsSimulationWebApi.Application.Simulations;

public sealed class Simulation
{
    public Simulation() {}

    public async Task<RunSimulationResponse> RunAsync(RunSimulationCommand command)
    {
        // Energy Balance on Solar Collector
        // E_in - E_out = 0 (steady state)
        // Gs + Gatm - Qconv - Qu - E = 0

        SolarPanel solarPanel = new(
            command.SolarPanelLength,
            command.SolarPanelSurfaceTemperature);
        
        Environment environment = new(
             command.SolarIrradiation,
             solarPanel.SolarAbsorptivity,
             command.AirTemperature,
             1.0M);

        StorageTank storageTank = new(
            command.StorageTankHeight,
            command.StorageTankRadius,
            command.InitialTankFluidTemperature,
            WaterProperties.Density,
            command.TankFluidMassFlowRate);
        
        // Qu = Gs + Gatm - Qconv - E (steady state)
        decimal usefulHeat = UsefulHeat(solarPanel, environment, command);
        
        decimal secondsPerHour = 3600.0M;
        decimal hoursPerDay = 24.0M;
        var times = new decimal[(int)((hoursPerDay * secondsPerHour) + 1)];

        RunSimulationResponse response = new();
        
        decimal fluidTempToSolarPanel = storageTank.InitialFluidTemperature;
        for (int i = 0; i < times.Length; i++)
        {
            DataPoint dataPoint = new();
            dataPoint.Time = i;
            
            // Update the film temperature (average of inlet fluid and panel surface temperature)
            decimal filmTemperature = (fluidTempToSolarPanel + solarPanel.SurfaceTemperature) / 2;
            
            decimal outletSolarCollectorFluidTemp = OutletSolarCollectorFluidTemp(usefulHeat, storageTank.MassFlowRate, filmTemperature);
            
            decimal tankTemp = TankTemperature(
                dataPoint.Time, 
                outletSolarCollectorFluidTemp,
                storageTank.InitialFluidTemperature,
                storageTank.MassFlowRate,
                storageTank.FluidMass);
            
            dataPoint.TankTemperature = tankTemp;

            if (response.DataPoints is null)
            {
                response.DataPoints = new List<DataPoint>();
            }
            
            response.DataPoints.Add(dataPoint);

            fluidTempToSolarPanel = tankTemp;
        }
        
        return response;
    }

    private decimal UsefulHeat(SolarPanel solarPanel, Environment environment, RunSimulationCommand command)
    {
        ThermophysicalProperties airProperties = AirProperties.Instance.AirPropertiesAtTempInKelvin(command.FilmTemperature);
        
        decimal absorbedSolarIrradiation = solarPanel.SolarAbsorptivity * environment.SolarIrradiation;
        decimal absorbedAtmosphericIrradiation = (solarPanel.SurfaceEmissivity * environment.AtmosphericIrradiation);
        decimal convectiveHeatTransferToAir = Equations.ConvectiveHeatTransfer(command, airProperties);
        decimal emissivePowerOfPanel = Equations.EmissivePower(0.1M, command.SolarPanelSurfaceTemperature);
        
        decimal usefulHeat = absorbedSolarIrradiation + absorbedAtmosphericIrradiation - convectiveHeatTransferToAir - emissivePowerOfPanel;
        return usefulHeat;
    }

    private decimal OutletSolarCollectorFluidTemp(decimal usefulHeat, decimal massFlowRate, decimal inletFluidTempToSolarPanel)
    {
        decimal outletSolarCollectorFluidTemp = (usefulHeat / (WaterProperties.HeatCapacity * massFlowRate)) + inletFluidTempToSolarPanel;
        return outletSolarCollectorFluidTemp;
    }

    private decimal TankTemperature(
        decimal time, 
        decimal inletFluidTemperature,
        decimal initialTankFluidTemperature,
        decimal massFlowRate,
        decimal mass)
    {
        return inletFluidTemperature - (inletFluidTemperature - initialTankFluidTemperature) * (decimal)Math.Exp(-(double)massFlowRate * (double)time / (double)mass);
    }
}