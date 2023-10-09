namespace RepoVault.Infrastructure.Backup;

public interface IBackupRepository
{
    public void CreateFullBackup(string token, string repoName);
    public void ShowRepoBackups();
    public void CreateRemoteBackup(string repositoryName, string token);
}