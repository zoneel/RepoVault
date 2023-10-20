using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepoVault.Application.Git;
using RepoVault.CLI;
using RepoVault.CLI.UserInteraction;
using RepoVault.Infrastructure.Backup;
using RepoVault.Infrastructure.Database;

#region Services Initialization
var host = ServiceRegistration.CreateHostBuilder(args).Build();
var serviceProvider = host.Services;
var userInteraction = serviceProvider.GetRequiredService<IUserInteractionService>();
var gitService = serviceProvider.GetRequiredService<IGitService>();
#endregion

#region Database Initialization

var context = serviceProvider.GetRequiredService<RepoVaultDbContext>();
context.Database.EnsureCreated();

#endregion

#region Main Pipeline

userInteraction.ShowMenu();
userInteraction.AuthenticateUser(out var token, out var gitServices); 
await gitService.InitializeToken(token);
var backupRepository = serviceProvider.GetRequiredService<IBackupRepository>();

var quit = false;
while (!quit)
{
    userInteraction.ChooseAction(out var response);
    switch (response)
    {
        case "1":
            await userInteraction.ShowRepositoriesPipeline(userInteraction, gitServices, token, backupRepository);
            break;
        case "2":
            userInteraction.ShowBackupsPipeline(userInteraction, token, backupRepository);
            break;
        case "3":
            Console.WriteLine("Goodbye!");
            quit = true;
            break;
        default:
            Console.WriteLine("Invalid input. Please try again!");
            break;
    }
    
#endregion

}



