namespace RepoVault.Application.Backup;

public interface IBackupService
{
    public void CreateBackupFolder(string repoName, out string repoBackupFolderPath);
    public void CreateBackupRepoFile(string repoName, string repoBackupFolderPath);
    public Task CreateBackupIssuesFile(string repoName);
}