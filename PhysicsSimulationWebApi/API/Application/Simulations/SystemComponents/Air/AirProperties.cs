using PassiveLogicPhysicsSimulation.Empirical;

namespace PhysicsSimulationWebApi.Application.Simulations.SystemComponents.Air;

public sealed class AirProperties
{
    // Lazy initialization of the singleton instance (thread safe) => don't need multiple of these initialized
    private static readonly Lazy<AirProperties> _lazy = new Lazy<AirProperties>(() => new AirProperties());
    
    // Public static property to access the instance
    public static AirProperties Instance => _lazy.Value;
    
    private List<ThermophysicalProperties> _airThermophysicalProperties { get; init; }

    private AirProperties()
    {
        // T. L. Bergman et al., "Fundamentals of Heat Transfer," 7th ed. Hoboken, NJ: Wiley, 2011, p. [995, Table A.4].
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Application", "Simulations", "SystemComponents", "Air"); 
        List<ThermophysicalProperties> airThermophysicalProperties =
            File.ReadAllLines(Path.Combine(path, "AirThermophysicalProperties.csv")) 
                .Skip(1)
                .Select(row => new ThermophysicalProperties(row))
                .ToList();
        
        _airThermophysicalProperties = airThermophysicalProperties;
    }

    public ThermophysicalProperties AirPropertiesAtTempInKelvin(decimal temperatureInKelvin)
    {
        ThermophysicalProperties? properties = _airThermophysicalProperties
            .FirstOrDefault(properties => properties.Temperature.Equals(temperatureInKelvin));

        if (properties is null)
        {
            ThermophysicalProperties interpolatedProperties = InterpolateProperties(temperatureInKelvin);
            return interpolatedProperties;
        }
        
        return properties;
    }

    private ThermophysicalProperties InterpolateProperties(decimal targetTemperatureInKelvin)
    {
        List<ThermophysicalProperties> orderedProperties = _airThermophysicalProperties
            .OrderBy(p => p.Temperature).ToList();
        
        if (targetTemperatureInKelvin <= orderedProperties.First().Temperature)
        {
            return orderedProperties.First();
        }
        
        if (targetTemperatureInKelvin >= orderedProperties.Last().Temperature)
        {
            return orderedProperties.Last();
        }
        
        int lowerIndex = 0;
        int upperIndex = orderedProperties.Count - 1;
        
        while (lowerIndex <= upperIndex)
        {
            int mid = (lowerIndex + upperIndex) / 2;

            if (orderedProperties.ElementAt(mid).Temperature < targetTemperatureInKelvin)
            {
                lowerIndex = mid + 1;
            }
            else
            {
                upperIndex = mid - 1;
            }
        }
        
        ThermophysicalProperties lower = orderedProperties.ElementAt(lowerIndex - 1);
        ThermophysicalProperties upper = orderedProperties.ElementAt(lowerIndex);
        
        decimal interpolatedPrandtl = Interpolate(
            lower.Prandtl, 
            upper.Prandtl, 
            lower.Temperature, 
            upper.Temperature, 
            targetTemperatureInKelvin);
        
        decimal interpolatedThermalConductivity = Interpolate(
            lower.ThermalConductivity, 
            upper.ThermalConductivity, 
            lower.Temperature, 
            upper.Temperature, 
            targetTemperatureInKelvin);
        
        decimal interpolatedKinematicViscosity = Interpolate(
            lower.KinematicViscosity, 
            upper.KinematicViscosity, 
            lower.Temperature, 
            upper.Temperature, 
            targetTemperatureInKelvin);
        
        return new ThermophysicalProperties
        {
            Temperature = targetTemperatureInKelvin,
            Prandtl = interpolatedPrandtl,
            ThermalConductivity = interpolatedThermalConductivity,
            KinematicViscosity = interpolatedKinematicViscosity
        };
    }

    private decimal Interpolate(decimal y1, decimal y2, decimal x1, decimal x2, decimal x)
    {
        return y1 + ((x - x1) * (y2 - y1)) / (x2 - x1);
    }
}