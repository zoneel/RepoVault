using Octokit;
using RepoVault.Application.Encryption;
using RepoVault.Application.Git;
using RepoVault.CLI;
using RepoVault.Infrastructure.Backup;
using RepoVault.Infrastructure.Database;
using RepoVault.Infrastructure.Services;
//pipeline
UserInteraction.ShowMenu();
UserInteraction.AuthenticateUser(out string token, out GitRepository gitServices);
BackupRepository backupRepository = new(new GitService(token), new EncryptionService(), new RepoVaultDbRepository(new RepoVaultDbContext()));

bool quit = false;
while (!quit)
{
    UserInteraction.ChooseAction(out string response);
    switch (response)
    {
        case "1":
            Console.WriteLine("Here are your repositories: ");
            UserInteraction.ShowUserRepositories(gitServices, token);
            UserInteraction.ShowStyledResponse("Enter name of repository you want to see the issues for: ");
            string repoName = Console.ReadLine();
            await UserInteraction.ShowRepoIssues(gitServices, token, repoName);
            UserInteraction.ShowStyledResponse("Do you want to create backup of this repository and it's issues? (y/n): ");
            string answer = Console.ReadLine();
            if (answer == "y")
            {
                backupRepository.CreateFullBackup(token, repoName);
            }
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








