using RepoVault.Domain.Entities;

namespace RepoVault.Infrastructure.Services;

public interface IGitRepository
{
    public bool UserIsAuthenticated(string token);
    public bool CheckIfRepositoryExists(string repositoryName);
    public Task<string> GetAuthenticatedUserLoginAsync(string token);
    public Task<IReadOnlyList<string>> ShowAllReposNamesAsync(string token);
    public Task<IReadOnlyList<IssueDTO>> ShowAllIssueForRepoAsync(string token, string repositoryName);
    public Task<long> GetRepositoryIdAsync(string token, string repoName);
}