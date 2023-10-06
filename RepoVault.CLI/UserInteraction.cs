using RepoVault.Infrastructure.Services;

namespace RepoVault.CLI;

public class UserInteraction
{
    public static void checkUserToken(string token, out GitRepository gitRepository)
    {

        gitRepository = new(token);
        if(gitRepository.UserIsAuthenticated(token))
        {
            Console.WriteLine("Welcome, you are logged in as "+gitRepository.GetAuthenticatedUserLogin(token).Result);
        }
        else
        {
            Console.WriteLine("You are not authenticated!");
            token = null;
            gitRepository = null;
            return;
        }
    }
    
    public static void ShowUserRepositories(GitRepository gitServices1, string s)
    {
        var listnum = 1;
        foreach (var repo in gitServices1.ShowAllReposNames(s).Result)
        {
            Console.WriteLine(listnum + ". " + repo);
            listnum++;
        }
    }

    public static async Task ShowRepoIssues(GitRepository gitRepository, string token, string repoName)
    {
            if (!gitRepository.CheckIfRepositoryExists(repoName))
            {
                Console.WriteLine("Repository does not exist. Please try again.");
                return;
            }
        var repositoryId =  await gitRepository.GetRepositoryId(token,repoName);
        var issues = await gitRepository.ShowAllIssueForRepo(token,repositoryId);

        foreach (var issue in issues)
        {
            Console.WriteLine($"{issue.Title} - [Created At: {issue.CreatedAt}]");
        }
    }
    
    
}