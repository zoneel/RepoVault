using Octokit;
//using RepoVault.Domain.Entities;

namespace RepoVault.Infrastructure.Services;

public class GitServices
{
    public bool UserIsAuthenticated(string token)
    {
        Application.Git.GitService gitService = new(token);
        try
        {
            return gitService.GetAuthenticatedUserLogin().Result != null;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> GetAuthenticatedUserLogin(string token)
    {
        Application.Git.GitService gitService = new(token);
        return await gitService.GetAuthenticatedUserLogin();
    }
    
    public async Task<IReadOnlyList<string>> ShowAllReposNames(string token)
    {
        Application.Git.GitService gitService = new(token);
        var fullData = await gitService.GetAllRepositoriesData();
        List<string> repoNames = new();
        foreach(var repo in fullData)
        {
            repoNames.Add(repo.Name);
        }
        return repoNames;
    }
}