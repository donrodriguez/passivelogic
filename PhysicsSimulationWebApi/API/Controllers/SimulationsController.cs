using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PhysicsSimulationWebApi.Application.Simulations;
using PhysicsSimulationWebApi.Application.Simulations.Commands.RunSimulation;

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
        RunSimulationResponse response = await simulation.RunAsync(command);
        
        // if (queryResult is null)
        // {
        //     return TypedResults.Problem(detail: "Getting user account profile failed", statusCode: 500);
        // }
        
        return TypedResults.Ok(response);
    }
}