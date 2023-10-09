using RepoVault.Application.Git;
using RepoVault.Domain.Entities;

namespace RepoVault.Infrastructure.Services;

public class GitRepository : IGitRepository
{
    public bool UserIsAuthenticated(string token)
    {
        try
        {
            return _gitService.GetAuthenticatedUserLogin().Result != null;
        }
        catch
        {
            return false;
        }
    }
    
    public bool CheckIfRepositoryExists(string repositoryName)
    {
        var repositories = _gitService.GetAllRepositoriesNames().Result;
        if (repositories.Contains(repositoryName))
            return true;
        return false;
    }

    public async Task<string> GetAuthenticatedUserLogin(string token)
    {
        GitService gitService = new(token);
        return await _gitService.GetAuthenticatedUserLogin();
    }

    public async Task<IReadOnlyList<string>> ShowAllReposNames(string token)
    {
        var fullData = await _gitService.GetAllRepositoriesData();
        List<string> repoNames = new();
        foreach (var repo in fullData) repoNames.Add(repo.Name);
        return repoNames;
    }

    public async Task<IReadOnlyList<IssueDTO>> ShowAllIssueForRepo(string token, string repositoryName)
    {
        var issues = await _gitService.GetAllIssuesForRepository(repositoryName);
        return issues;
    }

    public async Task<long> GetRepositoryId(string token, string repoName)
    {
        var fullData = await _gitService.GetAllRepositoriesData();
        foreach (var repo in fullData)
            if (repo.Name == repoName)
                return repo.Id;
        return 0;
    }

    #region Constructor and Dependencies

    private readonly string _token;
    private readonly IGitService _gitService;

    public GitRepository(string token)
    {
        _gitService = new GitService(token);
        _token = token;
    }

    #endregion
}