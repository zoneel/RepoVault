using System.Text.Json;
using Octokit;
using RepoVault.Domain.Entities;
using RepoVault.Domain.Exceptions;

namespace RepoVault.Application.Git;

public class GitService : IGitService
{
    #region Constructor and Dependencies

    private readonly GitHubClient _githubClient;

    public GitService()
    {
        _githubClient = new GitHubClient(new ProductHeaderValue("RepoVault"));
    }

    #endregion

    #region Methods

    // Method to get the authenticated user's login
    public async Task<string> GetAuthenticatedUserLoginAsync()
    {
        var user = await _githubClient.User.Current();
        return user.Login;
    }

    // Method to get all repositories for the authenticated user
    public async Task<IReadOnlyList<string>> GetAllRepositoriesNamesAsync()
    {
        var repositories = await _githubClient.Repository.GetAllForCurrent();

        List<string> repoNames = new();

        foreach (var repo in repositories) repoNames.Add(repo.Name);

        return repoNames;
    }

    // Method to get all repositories for the authenticated user
    public async Task<IReadOnlyList<RepositoryDto>> GetAllRepositoriesDataAsync()
    {
        var repositories = await _githubClient.Repository.GetAllForCurrent();

        List<RepositoryDto> repositoriesFullData = new();

        foreach (var repo in repositories)
        {
            var repoDto = new RepositoryDto(repo.Id, repo.Name, repo.Description, repo.Language, repo.CreatedAt,
                repo.UpdatedAt, repo.Size, repo.OpenIssuesCount);
            repositoriesFullData.Add(repoDto);
        }

        return repositoriesFullData;
    }

    // Method to get all issues for a repository
    public async Task<IReadOnlyList<IssueDto>> GetAllIssuesForRepositoryAsync(string repositoryName)
    {
        var userLogin = await GetAuthenticatedUserLoginAsync();
        var issues = await _githubClient.Issue.GetAllForRepository(userLogin, repositoryName);

        List<IssueDto> issuesFullData = new();

        foreach (var issue in issues)
        {
            var issueDto = new IssueDto(issue.Title, issue.Body, issue.State.Value.ToString(),
                issue.CreatedAt.ToString(), issue.UpdatedAt.ToString(), issue.ClosedAt.ToString(), issue.User.Login,
                issue.Url);
            issuesFullData.Add(issueDto);
        }

        return issuesFullData;
    }

    // Method to get all data for a repository
    public async Task<RepositoryDto> GetAllDataForRepositoryAsync(string repositoryName)
    {
        var userLogin = await GetAuthenticatedUserLoginAsync();
        var repository = await _githubClient.Repository.Get(userLogin, repositoryName);

        var repoDto = new RepositoryDto(repository.Id, repository.Name, repository.Description, repository.Language,
            repository.CreatedAt, repository.UpdatedAt, repository.Size, repository.OpenIssuesCount);
        return repoDto;
    }

    // Method to upload a repository to Github
    public async Task UploadRemoteRepositoryAsync(string repositoryName)
    {
        var newRepo = new NewRepository(repositoryName)
        {
            Private = true
        };
        var repoCreated = await _githubClient.Repository.Create(newRepo);
        Console.WriteLine($"Created new remote backup at {repoCreated.HtmlUrl}");
        var backupFolderPath = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory) ?? string.Empty, "RepoVaultBackups");
        var path = Path.Combine(backupFolderPath, repositoryName).Replace("_", " ");
        await UploadIssuesToRepositoryAsync(repoCreated.Owner.Login, repoCreated.Name, path);
    }

    // Method to upload issues to a repository
    public async Task UploadIssuesToRepositoryAsync(string owner, string repoName, string localRepositoryPath)
    {
        var directoryInfo = new DirectoryInfo(localRepositoryPath);
        foreach (var file in directoryInfo.GetFiles())
        {
            if (file.Name == "repo_backup.json") continue;
            var jsonContent = await File.ReadAllTextAsync(file.FullName);

            var myData = JsonSerializer.Deserialize<IssueDto>(jsonContent);

            if (myData != null)
            {
                var newIssue = new NewIssue(file.Name.Replace(".json", ""))
                {
                    Body = myData.Body // Safe to access Body if myData is not null
                };
                await _githubClient.Issue.Create(owner, repoName, newIssue);
            }
            else
            {
                throw new NullIssueException("Issue is null");
            }
        }
    }

    // Method to initialize the token
    public Task InitializeToken(string token)
    {
        _githubClient.Credentials = new Credentials(token);
        return Task.CompletedTask;
    }

    #endregion
}

