namespace PassiveLogicPhysicsSimulation.SystemComponents;

public class Environment
{
    public decimal SolarIrradiation { get; set; } // G_s 
    public decimal AtmosphericIrradiation { get; set; } // G_atm
    
    
    public decimal AirTemperature { get; set; } = 300.0M; 
    public decimal AirPressure { get; set; } = 1.0M;

}