namespace RepoVault.Application.Backup;

public interface IBackupService
{
    public void CreateBackupFolder(string repoName, out string repoBackupFolderPath);
    public void CreateBackupRepoFile(string repoName, string repoBackupFolderPath);
    public void CreateBackupIssuesFile(string repoName, string repoBackupFolderPath);
    public Dictionary<DateTime, string> GetFileNamesFromPath();
    public void CreateRemoteRepoFromBackup(string repositoryName, string token);
}