namespace RepoVault.Infrastructure.Backup;

public interface IBackupRepository
{
    public Task CreateFullBackup(string token, string repoName);
}