namespace ChatManagement.Endpoints
{
    public static class UserEndPoints
    {
        public static void MapUserEndPoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/users/start-session", async (Service.IServiceProvider businessServiceProvider, Service.Dto.LoginDto request) =>
            {
                var token = await businessServiceProvider.UserService!.AuthenticateUserAsync(request);
              
                return Results.Ok(new { Token = token.Item1, Username = request.UserName, MessageId = token.Item2 });
            })
            .WithTags("Users")
            .WithName("start-session")
            .WithSummary("Start the Chat Session");

        }
    }
}
