using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PhysicsSimulationWebApi.Application.Simulations;
using PhysicsSimulationWebApi.Application.Simulations.Commands.RunSimulation;
using PhysicsSimulationWebApi.Shared;

namespace PhysicsSimulationWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SimulationsController
{
    [HttpPost]
    [ProducesResponseType(500)]
    [ProducesResponseType(201), Produces(typeof(RunSimulationResponse))]
    [Route("run")]
    public async Task<Results<Ok<RunSimulationResponse>, BadRequest<string>, ProblemHttpResult>> RunAsync([FromBody] RunSimulationCommand command)
    {
        Simulation simulation = new();
        Result<RunSimulationResponse> response = await simulation.RunAsync(command);
        
        if (response.IsFailure)
        {
            return TypedResults.Problem(detail: "Simulation Failed", statusCode: 500);
        }
        
        return TypedResults.Ok(response.Value);
    }
}