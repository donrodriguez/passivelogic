using PassiveLogicPhysicsSimulation.Empirical;

namespace PassiveLogicPhysicsSimulation.SystemComponents.Air;

public sealed class AirProperties
{
    // Lazy initialization of the singleton instance (thread safe) => don't need multiple of these initialized
    private static readonly Lazy<AirProperties> _lazy =
        new Lazy<AirProperties>(() => new AirProperties());
    
    // Public static property to access the instance
    public static AirProperties Instance => _lazy.Value;
    
    private List<ThermophysicalProperties> _airThermophysicalProperties { get; init; }

    private AirProperties()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SystemComponents", "Air"); 
        // T. L. Bergman et al., "Fundamentals of Heat Transfer," 7th ed. Hoboken, NJ: Wiley, 2011, p. [995, Table A.4].
        List<ThermophysicalProperties> airThermophysicalProperties =
            File.ReadAllLines(Path.Combine(path, "AirThermophysicalProperties.csv")) 
                .Skip(1)
                .Select(row => new ThermophysicalProperties(row))
                .ToList();
        
        _airThermophysicalProperties = airThermophysicalProperties;
    }

    public ThermophysicalProperties AirPropertiesAtTempInKelvin(decimal temperatureInKelvin)
    {
        return _airThermophysicalProperties
            .FirstOrDefault(properties => properties.Temperature.Equals(temperatureInKelvin)) ?? new ThermophysicalProperties();
    }
}