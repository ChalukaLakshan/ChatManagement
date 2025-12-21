namespace ChatManagement.Endpoints
{
    public static class AgentEndPoints
    {
        public static void MapAgentEndPoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/agents", async (Service.IServiceProvider businessServiceProvider, Service.Dto.CreateAgentDto request) =>
            {
                var agentId = await businessServiceProvider.AgentService!.CreaAgentAsync(request);
                return Results.Created($"/api/agents/{agentId}", new { Id = agentId });
            })
            .WithTags("Agents")
            .WithName("CreateAgent")
            .WithSummary("Creates a new agent")
            .WithDescription("Creates a new agent in the system and returns the created agent's ID.");

            app.MapPost("/api/agents/authenticate", async (Service.IServiceProvider businessServiceProvider, Service.Dto.LoginDto request) =>
            {
                var token = await businessServiceProvider.AgentService!.AuthenticateAgentAsync(request);
                if (token is null)
                {
                    return Results.Unauthorized();
                }
                return Results.Ok(new { Token = token, Username = request.UserName });
            })
            .WithTags("Agents")
            .WithName("Authenticate")
            .WithSummary("Authenticate an agent");
        }
    }
}
