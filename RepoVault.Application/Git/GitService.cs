using Octokit;
using RepoVault.Domain.Entities;

namespace RepoVault.Application.Git;

public class GitService : IGitService
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
        var repositories = await _githubClient.Repository.GetAllForCurrent();
        
        List<string> repoNames = new();
        
        foreach(var repo in repositories)
        {
            repoNames.Add(repo.Name);
        }
        
        return repoNames;
    }
    
    // Method to get all repositories for the authenticated user
    public async Task<IReadOnlyList<RepositoryDTO>> GetAllRepositoriesData()
    {
        var repositories = await _githubClient.Repository.GetAllForCurrent();
        
        List<RepositoryDTO> repositoriesFullData = new();
        
        foreach(var repo in repositories)
        {
            var repoDTO = new RepositoryDTO(repo.Id, repo.Name, repo.Description, repo.Language, repo.CreatedAt, repo.UpdatedAt, repo.Size, repo.OpenIssuesCount);
            repositoriesFullData.Add(repoDTO);
        }
        
        return repositoriesFullData;
    }

    // Method to get all issues for a repository
    public async Task<IReadOnlyList<IssueDTO>> GetAllIssuesForRepository(string repositoryName)
    {
        var userLogin = await GetAuthenticatedUserLogin();
        var issues = await _githubClient.Issue.GetAllForRepository(userLogin,repositoryName);
        
        List<IssueDTO> issuesFullData = new();
        
        foreach(var issue in issues)
        {
            var issueDTO = new IssueDTO(issue.Title, issue.Body, issue.State.Value.ToString(), issue.CreatedAt.ToString(), issue.UpdatedAt.ToString(), issue.ClosedAt.ToString(), issue.User.Login, issue.Url);
            issuesFullData.Add(issueDTO);
        }
        
        return issuesFullData;
    }

    public async Task<RepositoryDTO> GetAllDataForRepository(string repositoryName)
    {
        var userLogin = await GetAuthenticatedUserLogin();
        var repository = await _githubClient.Repository.Get(userLogin, repositoryName);
        
        var repoDTO = new RepositoryDTO(repository.Id, repository.Name, repository.Description, repository.Language, repository.CreatedAt, repository.UpdatedAt, repository.Size, repository.OpenIssuesCount);
        return repoDTO;
    }
    
    public async Task<long> GetRepositoryId(string repoName)
    {
        var fullData = await GetAllRepositoriesData();
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