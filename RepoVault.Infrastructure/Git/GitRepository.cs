using Octokit;
using RepoVault.Application.Git;
using RepoVault.Domain.Entities;
namespace RepoVault.Infrastructure.Services;

public class GitRepository
{
    private readonly string _token;
    private readonly IGitService _gitService;

    public GitRepository(string token)
    {
        _gitService = new GitService(token);
        _token = token;
    }
    
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
        if(repositories.Contains(repositoryName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<string> GetAuthenticatedUserLogin(string token)
    {
        Application.Git.GitService gitService = new(token);
        return await _gitService.GetAuthenticatedUserLogin();
    }
    
    public async Task<IReadOnlyList<string>> ShowAllReposNames(string token)
    {
        var fullData = await _gitService.GetAllRepositoriesData();
        List<string> repoNames = new();
        foreach(var repo in fullData)
        {
            repoNames.Add(repo.Name);
        }
        return repoNames;
    }

    public async Task<IReadOnlyList<IssueDTO>> ShowAllIssueForRepo(string token,long repoId)
    {
        var issues = await _gitService.GetAllIssuesForRepository(repoId);
        return issues;
    }
    
    public async Task<long> GetRepositoryId(string token, string repoName)
    {
        var fullData = await _gitService.GetAllRepositoriesData();
        foreach(var repo in fullData)
        {
            if(repo.Name == repoName)
            {
                return repo.Id;
            }
        }
        return 0;
    } 
    
}