using EPGP.API.Services;
using EPGP.Data.DbContexts;
using EPGP.Data.Repositories;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins(configuration["AllowedCors"] ?? "*");
                      });
});

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
    .AddTransient<IAdminService, AdminService>()
    .AddTransient<IPointsService, PointsService>()
    .AddTransient<IPointsRepository, PointsRepository>()
    .AddTransient<IRaiderService, RaiderService>()
    .AddTransient<IRaiderRepository, RaiderRepository>()
    .AddTransient<IUploadsService, UploadsService>()
    .AddTransient<ILootHistoryRepository, LootHistoryRepository>();

builder.Services.AddControllers();

builder.Services
    .AddEntityFrameworkNpgsql()
    .AddDbContext<EPGPContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfire(config =>
                config.UsePostgreSqlStorage(configuration.GetConnectionString("HangfireConnection")));

builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger(c =>
{
    c.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
    {
        if (!httpRequest.Headers.ContainsKey("X-Forwarded-Host")) return;
        var basePath = "proxy";
        var serverUrl = $"{httpRequest.Scheme}://{httpRequest.Headers["X-Forwarded-Host"]}/{basePath}";
        swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
    });
});
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "";
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
    c.OAuthClientId(configuration["Authentication:ClientId"]);
});


app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.MapControllers();

app.UseHangfireDashboard();

app.Run();
