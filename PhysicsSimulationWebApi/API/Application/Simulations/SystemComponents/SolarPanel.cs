namespace PhysicsSimulationWebApi.Application.Simulations.SystemComponents;

public sealed class SolarPanel
{
    public decimal Length { get; set; }
    public decimal Width { get; set; }
    public decimal SolarAbsorptivity { get; set; }
    public decimal SurfaceEmissivity { get; set; }
    public decimal SurfaceTemperature { get; set; }

    public SolarPanel(
        decimal length,
        decimal surfaceTemperature)
    {
        Length = length;
        Width = length;
        SolarAbsorptivity = 0.95M; // alpha
        SurfaceEmissivity = 0.1M; // epsilon
        SurfaceTemperature = surfaceTemperature;
    }
}