
using System.Globalization;

namespace PassiveLogicPhysicsSimulation.Empirical;

public class ThermophysicalProperties
{
    public decimal Temperature { get; set; } // K
    public decimal Prandtl { get; set; } // dimensionless
    public decimal ThermalConductivity { get; set; } // W/(m-K)
    public decimal KinematicViscosity { get; set; } // m^2/s

    public ThermophysicalProperties() { }
    
    public ThermophysicalProperties(string csvRow)
    {
        var parsedCsvRow = FromCsv(csvRow);
        Temperature = decimal.Parse(parsedCsvRow[0], NumberStyles.Float, CultureInfo.InvariantCulture);
        Prandtl = decimal.Parse(parsedCsvRow[1], NumberStyles.Float, CultureInfo.InvariantCulture);
        ThermalConductivity = decimal.Parse(parsedCsvRow[2], NumberStyles.Float, CultureInfo.InvariantCulture);
        KinematicViscosity = decimal.Parse(parsedCsvRow[3], NumberStyles.Float, CultureInfo.InvariantCulture);
    }
    
    private string[] FromCsv(string csvRow)
    {
        string[] row = csvRow.Split(",");
        return row;
    }
}