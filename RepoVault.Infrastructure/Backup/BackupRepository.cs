using RepoVault.Application.Backup;
using RepoVault.Application.Git;

namespace RepoVault.Infrastructure.Backup;

public class BackupRepository : IBackupRepository
{
    private readonly BackupService _backupService;

    public BackupRepository()
    {
        _backupService = new BackupService("C:\\", new GitService("token"));
    }
    
    public async Task CreateFullBackup(string token, string repoName)
    {
        _backupService.CreateBackupFolder(repoName, out string repoBackupFolderPath);
        await _backupService.CreateBackupRepoFile(repoName, repoBackupFolderPath);
    }
}