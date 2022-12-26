// See https://aka.ms/new-console-template for more information
using EPGP.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var pointsRepo = new PointsRepository();
var raiderRepo = new RaiderRepository(pointsRepo);

raiderRepo.CreateRaider(new EPGP.Data.DbContexts.Raider
{
    CharacterName = "Raid",
    Realm = "Realm"
});


using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services.AddTransient<IRaiderRepository, RaiderRepository>()
        .AddTransient<IPointsRepository, PointsRepository>()
    )
    .Build();

await host.RunAsync();