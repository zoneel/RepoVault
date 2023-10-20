using RepoVault.Application.Git;
using RepoVault.Domain.Entities;

namespace RepoVault.Infrastructure.Git;

public class GitRepository : IGitRepository
{
    #region Constructor and Dependencies

    private readonly IGitService _gitService;

    public GitRepository(IGitService gitService)
    {
        _gitService = gitService;
    }

    #endregion

    private void AttachToken(string token)
    {
        _gitService.InitializeToken(token);
    }
    
    public bool UserIsAuthenticated(string token)
    {
        AttachToken(token);
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
        return await _gitService.GetAuthenticatedUserLoginAsync();
    }

    public async Task<IReadOnlyList<string>> ShowAllReposNamesAsync(string token)
    {
        var fullData = await _gitService.GetAllRepositoriesDataAsync();
        List<string> repoNames = new();
        foreach (var repo in fullData) repoNames.Add(repo.Name);
        return repoNames;
    }

    public async Task<IReadOnlyList<GithubIssue>> ShowAllIssueForRepoAsync(string token, string repositoryName)
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


}