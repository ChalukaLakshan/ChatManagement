using ChatManagement.Endpoints;
using ChatManagement.Middleware;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Service;


var builder = WebApplication.CreateBuilder(args);
var config = TypeAdapterConfig.GlobalSettings;
MapsterConfig.RegisterMapping();

builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
});
builder.Services.AddScoped<IBusinessServiceProvider, BusinessServiceProvider>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddSignalR();
var app = builder.Build();
app.UseCors("AngularPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setupAction => { setupAction.DocumentTitle = "chat-management-api"; });
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.MapAgentEndPoints();
app.MapUserEndPoints();

app.MapHub<SupportChatHub>("/supportHub");

app.UseExceptionHandler();

app.Run();
