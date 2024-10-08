using PassiveLogicPhysicsSimulation.Empirical;

namespace PhysicsSimulationWebApi.Application.Simulations.Empirical;

public sealed class AverageConvectionCoefficient
{
    /// <summary>
    /// Calculates the average convection coefficient h (W/m^s-K).
    /// </summary>
    /// <param name="flowVelocity">The flow rate of the fluid (m/s).</param>
    /// <param name="properties">Properites of fluid.</param>
    /// <param name="length">Length of the solar collector (m).</param>
    /// <returns>The average convection coefficient h (W/m^s-K) = (Nu * k) / L.</returns>
    public static decimal Calculate(decimal flowVelocity, ThermophysicalProperties properties, decimal length)
    {
        decimal nusselt = NusseltAverageAtLaminarFlow(flowVelocity, properties.KinematicViscosity, properties.Prandtl, length);
        
        return (nusselt * properties.KinematicViscosity) / length;
    }

    /// <summary>
    /// Calculates the average Nusselt number for laminar flow where the Prandtl number >= 0.6.
    /// </summary>
    /// <param name="flowVelocity">The flow rate of the fluid (m/s).</param>
    /// <param name="kinematicViscosity">Kinematic Viscosity of the fluid (m^2/s).</param>
    /// <param name="prandtlNumber">Prandtl number of fluid.</param>
    /// <param name="length">Length of the solar collector (m).</param>
    /// <returns>The dimensionless Nusselt number.</returns>
    public static decimal NusseltAverageAtLaminarFlow(
        decimal flowVelocity, 
        decimal kinematicViscosity, 
        decimal prandtlNumber,
        decimal length)
    {
        decimal reynolds = ReynoldsNumber(flowVelocity, kinematicViscosity, length);

        if (IsLaminar(reynolds))
        {
            return 0.664M 
                   * (decimal)Math.Pow((double)reynolds, 1.0 / 2.0)
                   * (decimal)Math.Pow((double)prandtlNumber, 1.0 / 3.0);
        }

        return 0.0296M
               * (decimal)Math.Pow((double)reynolds, 4.0 / 5.0)
               * (decimal)Math.Pow((double)prandtlNumber, 1.0 / 3.0);
    }

    /// <summary>
    /// Calculates the Reynolds number.
    /// </summary>
    /// <param name="flowVelocity">The flow rate of the fluid (m/s).</param>
    /// <param name="kinematicViscosity">Kinematic Viscosity of the fluid (m^2/s).</param>
    /// <param name="length">Length of the solar collector (m).</param>
    /// <returns>The dimensionless Reynolds number.</returns>
    public static decimal ReynoldsNumber(decimal flowVelocity, decimal kinematicViscosity, decimal length)
    {
        return (flowVelocity * length) / kinematicViscosity;
    }

    public static bool IsLaminar(decimal reynoldsNumber)
    {
        decimal reynoldsTurbulentThreshold = 5.0M * 100000.0M;
        return reynoldsNumber <= reynoldsTurbulentThreshold;
    }
}