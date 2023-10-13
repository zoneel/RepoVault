using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octokit;
using RepoVault.Application;
using RepoVault.CLI.UserInteraction;
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
                services.AddTransient<IUserInteractionService, UserInteractionService>();

            });
}