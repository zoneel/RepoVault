﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepoVault.Infrastructure.Backup;
using RepoVault.Infrastructure.Database;
using RepoVault.Infrastructure.DatabaseRepository;
using RepoVault.Infrastructure.Git;

namespace RepoVault.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static void RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IBackupRepository, BackupRepository>();
        services.AddScoped<IRepoVaultDbRepository, RepoVaultDbRepository>();
        services.AddScoped<IGitRepository, GitRepository>();
        services.AddDbContext<RepoVaultDbContext>(options =>
        {
            options.UseSqlite("Data Source=RepoVault.db");
        });
    }
}