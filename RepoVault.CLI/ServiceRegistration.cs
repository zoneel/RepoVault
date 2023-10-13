using Microsoft.Extensions.Hosting;
using RepoVault.Application;
using RepoVault.Infrastructure;

namespace RepoVault.CLI;

public static class ServiceRegistration
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Register services for each layer
                services.RegisterApplicationServices();
                services.RegisterInfrastructureServices();

            });
}