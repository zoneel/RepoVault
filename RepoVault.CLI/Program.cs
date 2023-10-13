using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RepoVault.Application;
using RepoVault.Application.Encryption;
using RepoVault.Application.Git;
using RepoVault.CLI;
using RepoVault.CLI.UserInteraction;
using RepoVault.Infrastructure;
using RepoVault.Infrastructure.Backup;
using RepoVault.Infrastructure.Database;
using RepoVault.Infrastructure.DatabaseRepository;

    var host = ServiceRegistration.CreateHostBuilder(args).Build();
    var serviceProvider = host.Services;


var UserInteraction = serviceProvider.GetRequiredService<UserInteractionService>();
//pipeline
UserInteraction.ShowMenu();
UserInteraction.AuthenticateUser(out var token, out var gitServices);
BackupRepository backupRepository = new(new GitService(token), new EncryptionService(),
    new RepoVaultDbRepository(new RepoVaultDbContext()));

var quit = false;
while (!quit)
{
    UserInteraction.ChooseAction(out var response);
    switch (response)
    {
        case "1":
            Console.WriteLine("Here are your repositories: ");
            UserInteraction.ShowUserRepositories(gitServices, token);
            UserInteraction.ShowStyledResponse("Enter name of repository you want to see the issues for: ");
            var repoName = Console.ReadLine();
            await UserInteraction.ShowRepoIssues(gitServices, token, repoName);
            UserInteraction.ShowStyledResponse(
                "Do you want to create backup of this repository and it's issues? (y/n): ");
            var answer = Console.ReadLine();
            if (answer == "y") backupRepository.CreateFullBackup(token, repoName);
            break;
        case "2":
            Console.WriteLine("Here are your latest backups: ");
            UserInteraction.ShowLocalBackups(token);
            UserInteraction.ShowStyledResponse("Enter the name of repository you want to make remote backup for: ");
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