using PassiveLogicPhysicsSimulation.Empirical;
using PhysicsSimulationWebApi.Application.Simulations.Commands.RunSimulation;
using PhysicsSimulationWebApi.Application.Simulations.Empirical;

namespace PhysicsSimulationWebApi.Application.Simulations.Theoretical;

public static class Equations
{
    public static readonly decimal SteffanBoltzmanConstant = 5.670374E-8M; // W / (m^s * K^4)
    
    /// <summary>
    /// Calculates the convective heat transfer Q = h * A (T<sub>1</sub> - T<sub>2</sub>) where T<sub>1</sub> > T<sub>2</sub>.
    /// </summary>
    /// <param name="command">The RunSimulationCommand.</param>
    /// <param name="fluidProperties">Properties of fluid.</param>
    /// <returns>The convective heat transfer Q (W)</returns>
    public static decimal ConvectiveHeatTransfer(RunSimulationCommand command, ThermophysicalProperties fluidProperties)
    {
        decimal area = command.SolarPanelLength * command.SolarPanelLength;
        decimal convectiveHeatTransfer =
            AverageConvectionCoefficient.Calculate(command.AirVelocity, fluidProperties, command.SolarPanelLength)
            * area * (command.SolarPanelSurfaceTemperature - command.AirTemperature);
        
        return convectiveHeatTransfer;
    }

    /// <summary>
    /// Calculates the Emissive Power of a surface E [W/m^s] = epsilon * sigma * T<sub>s</sub><sup>4</sup> 
    /// </summary>
    /// <param name="surfaceEmissivity">Epsilon.</param>
    /// <param name="surfaceTemperature">Surface temperature (K).</param>
    /// <returns>The Emissive Power (W/m^s)</returns>
    public static decimal EmissivePower(decimal surfaceEmissivity, decimal surfaceTemperature)
    {
        return surfaceEmissivity * SteffanBoltzmanConstant * (decimal)Math.Pow((double)surfaceTemperature, 4.0);
    }
}