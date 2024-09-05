namespace PhysicsSimulationWebApi.Application.Simulations.SystemComponents;

public class StorageTank
{
    // Assuming Cylindrical Tank
    public decimal Height { get; set; }
    public decimal Radius { get; set; }
    public decimal Area => (decimal)Math.PI * (decimal) Math.Pow((double) Radius, 2.0);
    public decimal Volume => Area * Height;
    public decimal InitialFluidTemperature { get; set; }
    public decimal FluidDensity { get; set; }
    public decimal MassFlowRate { get; set; }
    
    ///<summary>
    /// Returns the fluid mass inside storage tank in kg.
    /// </summary>
    public decimal FluidMass => Volume * FluidDensity;
    
    ///<summary>
    /// Creates cylindrical tank.
    /// </summary>
    /// <param name="height">Height of cylindrical tank in meters m.</param>
    /// <param name="radius">Radius of cylindrical tank in meters m.</param>
    /// <param name="initialFluidTemperature">Starting fluid temperature (K).</param>
    /// <param name="fluidDensity">In (kg/m<sup>3</sup>).</param>
    /// <param name="massFlowRate">In (kg/s).</param>
    public StorageTank(decimal height, decimal radius, decimal initialFluidTemperature, decimal fluidDensity, decimal massFlowRate)
    {
        Height = height;
        Radius = radius;
        InitialFluidTemperature = initialFluidTemperature;
        FluidDensity = fluidDensity;
        MassFlowRate = massFlowRate;
    }
}