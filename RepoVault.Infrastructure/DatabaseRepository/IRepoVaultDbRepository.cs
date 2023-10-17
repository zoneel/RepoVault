using RepoVault.Infrastructure.Database.Models;

namespace RepoVault.Infrastructure.DatabaseRepository;

public interface IRepoVaultDbRepository
{
    public void AddBackupLog(BackupLog backupLog);
}