using Octokit;

namespace RepoVault.Application.Git;

public class GitService
{
    private readonly GitHubClient _githubClient;

    public GitService(string githubToken)
    {
        _githubClient = new GitHubClient(new ProductHeaderValue("RepoVault"));
        _githubClient.Credentials = new Credentials(githubToken);
    }
    
    // Method to get the authenticated user's login
    public async Task<string> GetAuthenticatedUserLogin()
    {
        var user = await _githubClient.User.Current();
        return user.Login;
    }

    // Method to get all repositories for the authenticated user
    public async Task<IReadOnlyList<string>> GetAllRepositoriesNames()
    {
        //return await _githubClient.Repository.GetAllForCurrent();
        var repositories = await _githubClient.Repository.GetAllForCurrent();
        
        List<string> repoNames = new();
        
        foreach(var repo in repositories)
        {
            repoNames.Add(repo.Name);
        }
        
        return repoNames;
    }

    // Method to get all issues for a repository
    public async Task<IReadOnlyList<Issue>> GetAllIssuesForRepository(long repositoryId)
    {
        return await _githubClient.Issue.GetAllForRepository(repositoryId);
    }

}