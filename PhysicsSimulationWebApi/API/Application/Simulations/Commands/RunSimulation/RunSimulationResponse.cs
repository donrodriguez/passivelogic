using System.Text.Json.Serialization;

namespace PhysicsSimulationWebApi.Application.Simulations.Commands.RunSimulation;

public sealed class RunSimulationResponse
{
    [JsonPropertyName("Time")]
    public decimal[] Time { get; set; }
    
    [JsonPropertyName("TankTemperature")]
    public List<decimal> TankTemperature { get; set; }
}