using RepoVault.Domain.Entities;

namespace RepoVault.Application.Git;

public interface IGitService
{
    public Task<string> GetAuthenticatedUserLogin();
    public Task<IReadOnlyList<string>> GetAllRepositoriesNames();
    public Task<IReadOnlyList<IssueDTO>> GetAllIssuesForRepository(string repositoryName);
    public Task<IReadOnlyList<RepositoryDTO>> GetAllRepositoriesData();
    public Task<RepositoryDTO> GetAllDataForRepository(string repositoryName);
    public Task UploadRemoteRepository(string repositoryName);
    public Task UploadIssuesToRepository(string owner, string repoName, string LocalRepositoryPath);
}