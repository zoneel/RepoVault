using RepoVault.Infrastructure.Services;

namespace RepoVault.CLI;

public class UserInteraction
{
    public static void checkUserToken(string token, out GitServices gitServices)
    {

        gitServices = new(token);
        if(gitServices.UserIsAuthenticated(token))
        {
            Console.WriteLine("Welcome, you are logged in as "+gitServices.GetAuthenticatedUserLogin(token).Result);
        }
        else
        {
            Console.WriteLine("You are not authenticated!");
            token = null;
            gitServices = null;
            return;
        }
    }
    
    public static void ShowUserRepositories(GitServices gitServices1, string s)
    {
        var listnum = 1;
        foreach (var repo in gitServices1.ShowAllReposNames(s).Result)
        {
            Console.WriteLine(listnum + ". " + repo);
            listnum++;
        }
    }

    public static async Task ShowRepoIssues(GitServices gitServices, string token, string repoName)
    {
            if (!gitServices.CheckIfRepositoryExists(repoName))
            {
                Console.WriteLine("Repository does not exist. Please try again.");
                return;
            }
        var repositoryId =  await gitServices.GetRepositoryId(token,repoName);
        var issues = await gitServices.ShowAllIssueForRepo(token,repositoryId);

        foreach (var issue in issues)
        {
            Console.WriteLine($"{issue.Title} - [Created At: {issue.CreatedAt}]");
        }
    }
    
    
}