using RepoVault.Application.Backup;
using RepoVault.Application.Encryption;
using RepoVault.Application.Git;

namespace RepoVault.Infrastructure.Backup;

public class BackupRepository : IBackupRepository
{
    private readonly BackupService _backupService;
    private readonly EncryptionService _encryptionService;

    public BackupRepository(IGitService gitService, EncryptionService encryptionService)
    {
        _backupService = new BackupService("C:\\",gitService);
        _encryptionService = encryptionService;
    }
    
    public void CreateFullBackup(string token, string repoName)
    {
        _backupService.CreateBackupFolder(repoName, out string repoBackupFolderPath);
        _backupService.CreateBackupRepoFile(repoName, repoBackupFolderPath);
        _backupService.CreateBackupIssuesFile(repoName, repoBackupFolderPath);
        _encryptionService.EncryptFolder(repoBackupFolderPath, token);
        Console.WriteLine("Encrypted backup created successfully!");
    }
}