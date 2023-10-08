namespace RepoVault.Infrastructure.Database;

public class RepoVaultDbRepository
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