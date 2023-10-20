using RepoVault.Domain.Entities;

namespace RepoVault.Application.Git;

public interface IGitService
{
    public Task<string> GetAuthenticatedUserLoginAsync();
    public Task<IReadOnlyList<string>> GetAllRepositoriesNamesAsync();
    public Task<IReadOnlyList<GithubIssue>> GetAllIssuesForRepositoryAsync(string repositoryName);
    public Task<IReadOnlyList<GithubRepository>> GetAllRepositoriesDataAsync();
    public Task<GithubRepository> GetAllDataForRepositoryAsync(string repositoryName);
    public Task UploadRemoteRepositoryAsync(string repositoryName);
    public Task UploadIssuesToRepositoryAsync(string owner, string repoName, string localRepositoryPath);
    public Task InitializeToken(string token);
}