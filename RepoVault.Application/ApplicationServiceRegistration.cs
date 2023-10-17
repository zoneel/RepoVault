using Microsoft.Extensions.DependencyInjection;
using RepoVault.Application.Backup;
using RepoVault.Application.Encryption;
using RepoVault.Application.Git;

namespace RepoVault.Application;

public static class ApplicationServiceRegistration
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IGitService, GitService>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IBackupService, BackupService>();
    }
}