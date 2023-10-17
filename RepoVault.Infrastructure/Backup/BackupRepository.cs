using RepoVault.Application.Backup;
using RepoVault.Application.Encryption;
using RepoVault.Infrastructure.Database;
using RepoVault.Infrastructure.Database.Models;
using RepoVault.Infrastructure.DatabaseRepository;

namespace RepoVault.Infrastructure.Backup;

public class BackupRepository : IBackupRepository
{
    #region Constructor and Dependencies

    private readonly IBackupService _backupService;
    private readonly IEncryptionService _encryptionService;
    private readonly IRepoVaultDbRepository _repoVaultDbRepository;

    public BackupRepository(IBackupService backupService, IEncryptionService encryptionService,
        IRepoVaultDbRepository repoVaultDbRepository)
    {
        _backupService = backupService;
        _encryptionService = encryptionService;
        _repoVaultDbRepository = repoVaultDbRepository;
    }

    #endregion

    #region Methods

    // Method to create a full backup of a repository
    public void CreateFullBackup(string token, string repoName)
    {
        _backupService.CreateBackupFolder(repoName, out var repoBackupFolderPath);
        _backupService.CreateBackupRepoFile(repoName, repoBackupFolderPath);
        _backupService.CreateBackupIssuesFile(repoName, repoBackupFolderPath);
        _encryptionService.EncryptFolderAsync(repoBackupFolderPath, token);
        Console.WriteLine("Encrypted backup created successfully!");
        _repoVaultDbRepository.AddBackupLog(new BackupLog(repoName, $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}"));
        Console.WriteLine("Added backup log to database successfully!");
    }

    // Method to create a backup of a repository
    public void ShowRepoBackups()
    {
        var backups = _backupService.GetFileNamesFromPath();

        var latestBackups = backups.GroupBy(kv => kv.Value)
            .Select(group =>
            {
                var latestBackup = group.MaxBy(kv => kv.Key);
                return new KeyValuePair<DateTime, string>(latestBackup.Key, latestBackup.Value);
            })
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        var keys = latestBackups.Keys.ToList();
        var values = latestBackups.Values.ToList();

        for (var i = 0; i < latestBackups.Count; i++)
        {
            var key = keys[i];
            var value = values[i];

            Console.WriteLine($"[{value}] - [{key}]");
        }
    }

    // Method to create a remote backup of a repository
    public void CreateRemoteBackup(string repositoryName, string token)
    {
        _backupService.CreateRemoteRepoFromBackup(repositoryName, token);
    }

    #endregion
}