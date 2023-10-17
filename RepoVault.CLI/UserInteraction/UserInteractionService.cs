using RepoVault.Infrastructure.Backup;
using RepoVault.Infrastructure.Git;

namespace RepoVault.CLI.UserInteraction;

public class UserInteractionService : IUserInteractionService
{
    private readonly IGitRepository _gitRepository;
    private readonly IBackupRepository _backupRepository;

    public UserInteractionService(IGitRepository gitRepository, IBackupRepository backupRepository)
    {
        _backupRepository = backupRepository;
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
    public void ChooseAction(out string response)
    {
        ShowStyledResponse("What would you like to do? ");
        Console.WriteLine("[1] See your repositories");
        Console.WriteLine("[2] See your backups");
        Console.WriteLine("[3] Exit");
        response = Console.ReadLine() ?? string.Empty;
    }

    public bool CheckUserToken(string token, out IGitRepository gitRepository)
    {
        if (_gitRepository.UserIsAuthenticated(token))
        {
            gitRepository = _gitRepository;
            return true;
        }

        gitRepository = null;
        return false;
    }

    // Authenticate user
    public  void AuthenticateUser(out string correctToken, out IGitRepository correctGitServices)
    {
        correctToken = null;
        correctGitServices = null;

        while (true)
        {
            ShowStyledResponse(
                "Paste your Github user token here (don't know how to get one? See guide that I've made: https://github.com/zoneel/RepoVault#how-to-create-my-token): ");
            var token = Console.ReadLine();

            if (CheckUserToken(token, out var gitServices))
            {
                correctToken = token;
                correctGitServices = gitServices;
                Console.WriteLine($"Successfully logged in as {gitServices.GetAuthenticatedUserLoginAsync(token).Result}");
                break;
            }

            Console.WriteLine("Your token is not working. Please try again!");
        }
    }

    // Show user repositories
    public  void ShowUserRepositories(IGitRepository gitServices1, string s)
    {
        var listNum = 1;
        var list = gitServices1.ShowAllReposNamesAsync(s).Result;
        foreach (var repo in list)
        {
            Console.WriteLine(listNum + ". " + repo);
            listNum++;
        }
    }

    // Show local backups
    public  void ShowLocalBackups(string s)
    {
        _backupRepository.ShowRepoBackups();
    }

    // Show all Issues that repository has
    public  async Task ShowRepoIssues(IGitRepository gitRepository, string token, string repoName)
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