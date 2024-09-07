using System.Text.Json.Serialization;

namespace PhysicsSimulationWebApi.Application.Simulations.Commands.RunSimulation;

public sealed class RunSimulationResponse
{
    [JsonPropertyName("DataPoints")]
    public List<DataPoint>? DataPoints { get; set; }
}

public sealed class DataPoint
{
    [JsonPropertyName("Time")]
    public decimal Time { get; set; }
    
    [JsonPropertyName("TankTemperature")]
    public decimal TankTemperature { get; set; }
} 