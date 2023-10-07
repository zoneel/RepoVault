using RepoVault.Application.Backup;
using RepoVault.Application.Git;

namespace RepoVault.Infrastructure.Backup;

public class BackupRepository : IBackupRepository
{
    private readonly BackupService _backupService;

    public BackupRepository(IGitService gitService)
    {
        _backupService = new BackupService("C:\\",gitService);
    }
    
    public void CreateFullBackup(string token, string repoName)
    {
        _backupService.CreateBackupFolder(repoName, out string repoBackupFolderPath);
        _backupService.CreateBackupRepoFile(repoName, repoBackupFolderPath);
    }
}