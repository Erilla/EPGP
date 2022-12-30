// See https://aka.ms/new-console-template for more information
using EPGP.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services.AddTransient<IRaiderRepository, RaiderRepository>()
        .AddTransient<IPointsRepository, PointsRepository>()
    )
    .Build();

await host.RunAsync();