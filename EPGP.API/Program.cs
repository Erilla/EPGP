using EPGP.API.Services;
using EPGP.Data.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "API",
                Version = "v1",
                Description = "A REST API",
                TermsOfService = new Uri("https://lmgtfy.com/?q=i+like+pie")
            });
});
builder.Services
    .AddTransient<IPointsService, PointsService>()
    .AddTransient<IPointsRepository, PointsRepository>()
    .AddTransient<IRaiderService, RaiderService>()
    .AddTransient<IRaiderRepository, RaiderRepository>()
    .AddTransient<IUploadsService, UploadsService>()
    .AddTransient<ILootHistoryRepository, LootHistoryRepository>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
        c.OAuthClientId(config["Authentication:ClientId"]);
    });
}


app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.Run();
