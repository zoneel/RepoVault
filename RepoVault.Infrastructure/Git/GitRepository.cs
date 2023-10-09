using RepoVault.Application.Git;
using RepoVault.Domain.Entities;

namespace RepoVault.Infrastructure.Services;

public class GitRepository : IGitRepository
{
    public bool UserIsAuthenticated(string token)
    {
        try
        {
            return _gitService.GetAuthenticatedUserLoginAsync().Result != null;
        }
        catch
        {
            return false;
        }
    }
    
    public bool CheckIfRepositoryExists(string repositoryName)
    {
        var repositories = _gitService.GetAllRepositoriesNamesAsync().Result;
        if (repositories.Contains(repositoryName))
            return true;
        return false;
    }

    public async Task<string> GetAuthenticatedUserLoginAsync(string token)
    {
        GitService gitService = new(token);
        return await _gitService.GetAuthenticatedUserLoginAsync();
    }

    public async Task<IReadOnlyList<string>> ShowAllReposNamesAsync(string token)
    {
        var fullData = await _gitService.GetAllRepositoriesDataAsync();
        List<string> repoNames = new();
        foreach (var repo in fullData) repoNames.Add(repo.Name);
        return repoNames;
    }

    public async Task<IReadOnlyList<IssueDTO>> ShowAllIssueForRepoAsync(string token, string repositoryName)
    {
        var issues = await _gitService.GetAllIssuesForRepositoryAsync(repositoryName);
        return issues;
    }

    public async Task<long> GetRepositoryIdAsync(string token, string repoName)
    {
        var fullData = await _gitService.GetAllRepositoriesDataAsync();
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