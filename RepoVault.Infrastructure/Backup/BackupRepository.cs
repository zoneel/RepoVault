using RepoVault.Application.Backup;
using RepoVault.Application.Encryption;
using RepoVault.Application.Git;
using RepoVault.Infrastructure.Database;

namespace RepoVault.Infrastructure.Backup;

public class BackupRepository : IBackupRepository
{
    private readonly BackupService _backupService;
    private readonly EncryptionService _encryptionService;
    private readonly RepoVaultDbRepository _repoVaultDbRepository;

    public BackupRepository(IGitService gitService, EncryptionService encryptionService, RepoVaultDbRepository repoVaultDbRepository)
    {
        _backupService = new BackupService("C:\\",gitService, encryptionService);
        _encryptionService = encryptionService;
        _repoVaultDbRepository = repoVaultDbRepository;
    }
    
    public void CreateFullBackup(string token, string repoName)
    {
        _backupService.CreateBackupFolder(repoName, out string repoBackupFolderPath);
        _backupService.CreateBackupRepoFile(repoName, repoBackupFolderPath);
        _backupService.CreateBackupIssuesFile(repoName, repoBackupFolderPath);
        _encryptionService.EncryptFolder(repoBackupFolderPath, token);
        Console.WriteLine("Encrypted backup created successfully!");
        _repoVaultDbRepository.AddBackupLog(new BackupLog(repoName, $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}"));
        Console.WriteLine("Added backup log to database successfully!");
    }
    
    public void ShowRepoBackups()
    {
        var backups = _backupService.GetFileNamesFromPath();

        var latestBackups = backups.GroupBy(kv => kv.Value)
            .Select(group =>
            {
                var latestBackup = group.OrderByDescending(kv => kv.Key).First();
                return new KeyValuePair<DateTime, string>(latestBackup.Key, latestBackup.Value);
            })
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        var keys = latestBackups.Keys.ToList();
        var values = latestBackups.Values.ToList();

        for (int i = 0; i < latestBackups.Count; i++)
        {
            DateTime key = keys[i];
            string value = values[i];
    
            Console.WriteLine($"[{value} {key}]");
        }
    }

    public void CreateRemoteBackup(string repositoryName, string token)
    {
        _backupService.CreateRemoteRepoFromBackup(repositoryName, token);
    }
}