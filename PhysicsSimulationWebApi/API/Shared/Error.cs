namespace PhysicsSimulationWebApi.Shared;

public sealed record Error(string Code, string? Description = default)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");
    public static readonly Error InvalidArgumentParams = new("Error.InvalidArgumentParams", "Invalid argument params");
    public static readonly Error NotFound = new("Error.NotFound", "Not found");
    public static readonly Error DivideByZero = new("Error.DivideByZero", "Cannot divide by zero");
    
    public static implicit operator Result(Error error) => Result.Failure(error);

    public Result ToResult() => Result.Failure(this);
}