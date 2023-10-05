using Octokit;

namespace RepoVault.Application.Git;

public interface IGitService
{
    public Task<string> GetAuthenticatedUserLogin();

    public Task<IReadOnlyList<string>> GetAllRepositoriesNames();

    public Task<IReadOnlyList<Issue>> GetAllIssuesForRepository(string owner, string repoName);
}

