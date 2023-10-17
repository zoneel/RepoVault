using RepoVault.Infrastructure.Database;
using RepoVault.Infrastructure.Database.Models;

namespace RepoVault.Infrastructure.DatabaseRepository;

public class RepoVaultDbRepository : IRepoVaultDbRepository
{
    private readonly RepoVaultDbContext _dbContext;

    public RepoVaultDbRepository(RepoVaultDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddBackupLog(BackupLog backupLog)
    {
        _dbContext.BackupLogs.Add(backupLog);
        _dbContext.SaveChanges();
    }
}