using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RepoVault.Application.Git;

namespace RepoVault.Application.Backup;

public class BackupService : IBackupService
{
    private readonly string backupFolderPath;
    private readonly IGitService _gitService;

    public BackupService(string backupBasePath, IGitService gitService)
    {
        backupFolderPath = Path.Combine(backupBasePath, "RepoVaultBackups");
        _gitService = gitService;
    }

    
    public void CreateBackupFolder(string repoName, out string repoBackupFolderPath)
    {
        if (!Directory.Exists(backupFolderPath))
        {
            Directory.CreateDirectory(backupFolderPath);
        }

        repoBackupFolderPath = Path.Combine(backupFolderPath, $"{repoName}-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}");
        if (!Directory.Exists(repoBackupFolderPath))
        {
            Directory.CreateDirectory(repoBackupFolderPath);
        }
    }

    public void CreateBackupRepoFile(string repoName, string repoBackupFolderPath)
    {
        string repoBackupFilePath = Path.Combine(repoBackupFolderPath, "repo_backup.json");
        var repoData =  _gitService.GetAllDataForRepository(repoName).Result;
        string json = JsonConvert.SerializeObject(repoData);
        File.WriteAllText(repoBackupFilePath, json);
    }

    public Task CreateBackupIssuesFile(string repoName)
    {
        throw new NotImplementedException();
    }
}