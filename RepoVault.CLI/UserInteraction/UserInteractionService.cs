using RepoVault.Application.Encryption;
using RepoVault.Application.Git;
using RepoVault.Infrastructure.Backup;
using RepoVault.Infrastructure.Database;
using RepoVault.Infrastructure.DatabaseRepository;
using RepoVault.Infrastructure.Services;

namespace RepoVault.CLI.UserInteraction;

public class UserInteractionService : IUserInteractionService
{
    private readonly IGitRepository _gitRepository;

    public UserInteractionService(IGitRepository gitRepository)
    {
        _gitRepository = gitRepository;
    }
    
    // Show the menu
    public  void ShowMenu()
    {
        // Store the current console foreground color
        var originalColor = Console.ForegroundColor;

        // Set the foreground color to green
        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine(@"
ooooooooo.                                oooooo     oooo                     oooo      .   
`888   `Y88.                               `888.     .8'                      `888    .o8   
 888   .d88'  .ooooo.  oo.ooooo.   .ooooo.  `888.   .8'  .oooo.   oooo  oooo   888  .o888oo 
 888ooo88P'  d88' `88b  888' `88b d88' `88b  `888. .8'  `P  )88b  `888  `888   888    888   
 888`88b.    888ooo888  888   888 888   888   `888.8'    .oP""888   888   888   888    888   
 888  `88b.  888    .o  888   888 888   888    `888'    d8(  888   888   888   888    888 . 
o888o  o888o `Y8bod8P'  888bod8P' `Y8bod8P'     `8'     `Y888""""8o  `V88V""V8P' o888o   ""888"" 
                        888                                                                     
                       o888o                                                                                                                                                                
");
        Console.ForegroundColor = originalColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Visit at github.com/zoneel/RepoVault");
        Console.WriteLine("Created by github.com/zoneel");
        Console.ForegroundColor = originalColor;
        Console.WriteLine(
            "================================================================================================");
        Console.WriteLine("Welcome to RepoVault!");
    }

    // Choose an action
    public  void ChooseAction(out string response)
    {
        ShowStyledResponse("What would you like to do? ");
        Console.WriteLine("[1] See your repositories");
        Console.WriteLine("[2] See your backups");
        Console.WriteLine("[3] Exit");
        response = Console.ReadLine();
    }

    // Check if user token is valid
    public bool CheckUserToken(string token, out GitRepository gitRepository)
    {
        gitRepository = new GitRepository(token);
        if (gitRepository.UserIsAuthenticated(token)) return true;

        token = null;
        gitRepository = null;
        return false;
    }

    // Authenticate user
    public  void AuthenticateUser(out string CorrectToken, out GitRepository CorrectgitServices)
    {
        CorrectToken = null;
        CorrectgitServices = null;

        while (true)
        {
            ShowStyledResponse(
                "Paste your Github user token here (don't know how to get one? See guide that I've made: https://github.com/zoneel/RepoVault#how-to-create-my-token): ");
            var token = Console.ReadLine();

            if (CheckUserToken(token, out var gitServices))
            {
                CorrectToken = token;
                CorrectgitServices = gitServices;
                Console.WriteLine($"Successfully logged in as {gitServices.GetAuthenticatedUserLoginAsync(token).Result}");
                break;
            }

            Console.WriteLine("Your token is not working. Please try again!");
        }
    }

    // Show user repositories
    public  void ShowUserRepositories(GitRepository gitServices1, string s)
    {
        var listnum = 1;
        foreach (var repo in gitServices1.ShowAllReposNamesAsync(s).Result)
        {
            Console.WriteLine(listnum + ". " + repo);
            listnum++;
        }
    }

    // Show local backups
    public  void ShowLocalBackups(string s)
    {
        BackupRepository backupRepository = new(new GitService(s), new EncryptionService(),
            new RepoVaultDbRepository(new RepoVaultDbContext()));
        backupRepository.ShowRepoBackups();
    }

    // Show all Issues that repository has
    public  async Task ShowRepoIssues(GitRepository gitRepository, string token, string repoName)
    {
        if (!gitRepository.CheckIfRepositoryExists(repoName))
        {
            Console.WriteLine("Repository does not exist. Please try again.");
            return;
        }

        var issues = await gitRepository.ShowAllIssueForRepoAsync(token, repoName);
        Console.WriteLine($"There are {issues.Count} issues in this repository.");
        foreach (var issue in issues) Console.WriteLine($"[{issue.Title}] - [Created At: {issue.CreatedAt}]");
    }

    // Show styled response
    public  void ShowStyledResponse(string text)
    {
        var originalColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(text);
        Console.ForegroundColor = originalColor;
    }
}