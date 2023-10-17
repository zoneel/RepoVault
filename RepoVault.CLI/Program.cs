using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepoVault.Application.Encryption;
using RepoVault.Application.Git;
using RepoVault.CLI;
using RepoVault.CLI.UserInteraction;
using RepoVault.Infrastructure.Backup;


    var host = ServiceRegistration.CreateHostBuilder(args).Build();
    var serviceProvider = host.Services;


var userInteraction = serviceProvider.GetRequiredService<IUserInteractionService>();
var gitService = serviceProvider.GetRequiredService<IGitService>();
//pipeline

#region Database Initialization

using var context = new RepoVaultDbContext(new DbContextOptionsBuilder<RepoVaultDbContext>()
    .UseSqlite("Data Source=RepoVault.db")
    .Options);

context.Database.EnsureCreated();

#endregion

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
            Console.WriteLine("Here are your repositories: ");
            userInteraction.ShowUserRepositories(gitServices, token);
            userInteraction.ShowStyledResponse("Enter name of repository you want to see the issues for: ");
            var repoName = Console.ReadLine();
            await userInteraction.ShowRepoIssues(gitServices, token, repoName);
            userInteraction.ShowStyledResponse(
                "Do you want to create backup of this repository and it's issues? (y/n): ");
            var answer = Console.ReadLine();
            if (answer == "y") backupRepository.CreateFullBackup(token, repoName);
            break;
        case "2":
            Console.WriteLine("Here are your latest backups: ");
            userInteraction.ShowLocalBackups(token);
            userInteraction.ShowStyledResponse("Enter the name of repository you want to make remote backup for: ");
            repoName = Console.ReadLine();
            backupRepository.CreateRemoteBackup(repoName, token);
            break;
        case "3":
            Console.WriteLine("Goodbye!");
            quit = true;
            break;
        default:
            Console.WriteLine("Invalid input. Please try again!");
            break;
    }
}