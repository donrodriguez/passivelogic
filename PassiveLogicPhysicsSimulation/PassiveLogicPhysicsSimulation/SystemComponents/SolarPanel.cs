namespace PassiveLogicPhysicsSimulation.SystemComponents;

public class SolarPanel
{
    public decimal Length { get; set; }
    public decimal Width { get; set; }

    public SolarPanel(decimal length, decimal width)
    {
        Length = length;
        Width = width;
    }
}