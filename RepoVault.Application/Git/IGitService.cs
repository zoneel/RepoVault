using RepoVault.Domain.Entities;

namespace RepoVault.Application.Git;

public interface IGitService
{
    public Task<string> GetAuthenticatedUserLoginAsync();
    public Task<IReadOnlyList<string>> GetAllRepositoriesNamesAsync();
    public Task<IReadOnlyList<IssueDTO>> GetAllIssuesForRepositoryAsync(string repositoryName);
    public Task<IReadOnlyList<RepositoryDTO>> GetAllRepositoriesDataAsync();
    public Task<RepositoryDTO> GetAllDataForRepositoryAsync(string repositoryName);
    public Task UploadRemoteRepositoryAsync(string repositoryName);
    public Task UploadIssuesToRepositoryAsync(string owner, string repoName, string LocalRepositoryPath);
}