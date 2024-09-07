using PhysicsSimulationWebApi.Application.Simulations.Theoretical;

namespace PhysicsSimulationWebApi.Application.Simulations.SystemComponents;

public sealed class Environment
{
    public decimal SolarIrradiation { get; set; } // G_s (W/m^s)
    public decimal SkyTemperature { get; set; } = 263.15M; // T_sky (K)
    public decimal SkyAbsorptivity { get; set; } // dimensionless
    
    ///<summary>
    /// G_atm = sigma * T<sup>4</sup> (W/m^s)
    /// </summary>
    public decimal AtmosphericIrradiation 
        => Equations.SteffanBoltzmanConstant * (decimal) Math.Pow((double) SkyTemperature, 4.0);
    public decimal AirTemperature { get; set; } = 300.0M; 
    public decimal AirPressure { get; set; } = 1.0M;

    public Environment(
        decimal solarIrradiation,
        decimal skyAbsorptivity,
        decimal airTemperature,
        decimal airPressure)
    {
        SolarIrradiation = solarIrradiation;
        SkyAbsorptivity = skyAbsorptivity;
        AirTemperature = airTemperature;
        AirPressure = airPressure;
    }
}