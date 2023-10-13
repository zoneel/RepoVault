using RepoVault.Infrastructure.Database.Models;

namespace RepoVault.Infrastructure.Database;

public interface IRepoVaultDbRepository
{
    public void AddBackupLog(BackupLog backupLog);
}