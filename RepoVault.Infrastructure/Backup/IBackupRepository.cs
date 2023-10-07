namespace RepoVault.Infrastructure.Backup;

public interface IBackupRepository
{
    public void CreateFullBackup(string token, string repoName);
}