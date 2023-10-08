using RepoVault.Application.Encryption;
using RepoVault.Application.Git;
using RepoVault.CLI;
using RepoVault.Infrastructure.Backup;
using RepoVault.Infrastructure.Database;
using RepoVault.Infrastructure.Services;
//pipeline
Console.WriteLine(DateTime.Now);
UserInteraction.ShowMenu();
Console.WriteLine("Paste your Github user token here (don't know how to get one? Generate it here: github.com/settings/tokens): ");
string token = Console.ReadLine();
UserInteraction.checkUserToken(token, out GitRepository gitServices);

Console.WriteLine("What would you like to do? ");
Console.WriteLine("[1] See your repositories");
Console.WriteLine("[2] See your backups");
var response = Console.ReadLine();
switch (response)
{
    case "1":
        Console.WriteLine("Here are your repositories: ");
        UserInteraction.ShowUserRepositories(gitServices, token);
        Console.WriteLine("Enter name of repository you want to see the issues for: ");
        string repoName = Console.ReadLine();
        Console.WriteLine("Here are the issues for "+repoName+": ");
        await UserInteraction.ShowRepoIssues(gitServices, token, repoName);
        Console.WriteLine("Do you want to create backup of this Repository and it's issues? (y/n): ");
        string answer = Console.ReadLine();
        if (answer == "y")
        {
            BackupRepository backupRepository = new(new GitService(token), new EncryptionService(), new RepoVaultDbRepository(new RepoVaultDbContext()));
            backupRepository.CreateFullBackup(token, repoName);
        }
        break;
    case "2":
        Console.WriteLine("Here are your latest backups: ");
        UserInteraction.ShowLocalBackups(gitServices, token);
        break;
}







