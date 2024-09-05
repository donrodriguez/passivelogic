namespace PhysicsSimulationWebApi;

public static class ConfigServiceCollectionExtensions
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        string? policyName = configuration.GetSection("CorsPolicies:Name").Get<string>();
        string? origin = configuration.GetSection("CorsPolicies:Origin").Get<string>();
        services.AddCors(options =>
        {
            // Add the origin header so it will not be redacted
            options.AddPolicy(name: policyName,
                policy =>
                {
                    policy.WithOrigins(origin)
                        .WithHeaders("Content-Type")
                        .WithHeaders("Authorization")
                        .AllowAnyMethod(); // TODO: remove in production
                        
                }
            );
        });

        return services;
    }
}