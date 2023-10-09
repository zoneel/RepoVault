using RepoVault.Domain.Entities;

namespace RepoVault.Infrastructure.Services;

public interface IGitRepository
{
    public bool UserIsAuthenticated(string token);
    public bool CheckIfRepositoryExists(string repositoryName);
    public Task<string> GetAuthenticatedUserLogin(string token);
    public Task<IReadOnlyList<string>> ShowAllReposNames(string token);
    public Task<IReadOnlyList<IssueDTO>> ShowAllIssueForRepo(string token, string repositoryName);
    public Task<long> GetRepositoryId(string token, string repoName);
}