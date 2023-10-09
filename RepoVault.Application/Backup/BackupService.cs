using System.Globalization;
using Newtonsoft.Json;
using RepoVault.Application.Encryption;
using RepoVault.Application.Git;

namespace RepoVault.Application.Backup;

public class BackupService : IBackupService
{
    #region Constructor and Dependencies

    private readonly string backupFolderPath;
    private readonly IGitService _gitService;
    private readonly IEncryptionService _encryptionService;

    public BackupService(string backupBasePath, IGitService gitService, IEncryptionService encryptionService)
    {
        backupFolderPath = Path.Combine(backupBasePath, "RepoVaultBackups");
        _gitService = gitService;
        _encryptionService = encryptionService;
    }

    #endregion

    #region Methods

    // Method to create a backup folder
    public void CreateBackupFolder(string repoName, out string repoBackupFolderPath)
    {
        if (!Directory.Exists(backupFolderPath)) Directory.CreateDirectory(backupFolderPath);

        repoBackupFolderPath = Path.Combine(backupFolderPath, $"{repoName} {DateTime.Now:yyyy-MM-dd-HH-mm-ss}");
        if (!Directory.Exists(repoBackupFolderPath)) Directory.CreateDirectory(repoBackupFolderPath);
    }

    // Method to create a backup file for a repository
    public void CreateBackupRepoFile(string repoName, string repoBackupFolderPath)
    {
        var repoBackupFilePath = Path.Combine(repoBackupFolderPath, "repo_backup.json");
        var repoData = _gitService.GetAllDataForRepositoryAsync(repoName).Result;
        var json = JsonConvert.SerializeObject(repoData);
        File.WriteAllText(repoBackupFilePath, json);
    }

    public void CreateBackupIssuesFile(string repoName, string repoBackupFolderPath)
    {
        var issuesData = _gitService.GetAllIssuesForRepositoryAsync(repoName).Result;

        foreach (var issue in issuesData)
        {
            var json = JsonConvert.SerializeObject(issue);
            File.WriteAllText(Path.Combine(repoBackupFolderPath, $"{issue.Title}.json"), json);
        }
    }

    // Method to get the names of all backup folders
    public Dictionary<DateTime, string> GetFileNamesFromPath()
    {
        try
        {
            // Check if the directory exists
            if (!Directory.Exists(backupFolderPath))
            {
                Console.WriteLine("Directory not found.");
                return null;
            }

            // Get an array of directory paths
            var directoryPaths = Directory.GetDirectories(backupFolderPath);

            // Create a dictionary to store the results
            var directoryDictionary = new Dictionary<DateTime, string>();

            // Specify the format of the date in the directory name
            var dateFormat = "yyyy-MM-dd-HH-mm-ss";

            // Iterate through directory paths
            foreach (var dirPath in directoryPaths)
            {
                // Extract the directory name without the full path
                var directoryName = Path.GetFileName(dirPath);

                // Split the directory name into parts using space as the separator
                var parts = directoryName.Split(' ');

                // Check if there are enough parts to extract a date and repository name
                if (parts.Length >= 2)
                    // Attempt to parse the date part to DateTime using the specified format
                    if (DateTime.TryParseExact(parts[1], dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                            out var date))
                        directoryDictionary[date] = parts[0];
            }

            return directoryDictionary;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }

    // Method to create a remote repository from a backup
    public void CreateRemoteRepoFromBackup(string repositoryName, string token)
    {
        var backups = GetFileNamesFromPath();
        var latestBackups = backups.GroupBy(kv => kv.Value)
            .Select(group =>
            {
                var latestBackup = group.OrderByDescending(kv => kv.Key).First();
                return new KeyValuePair<DateTime, string>(latestBackup.Key, latestBackup.Value);
            })
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        //check if repositoryName is present in latestBackups
        if (latestBackups.ContainsValue(repositoryName))
        {
            //get the key of the latest backup
            var latestBackupKey = latestBackups.FirstOrDefault(x => x.Value == repositoryName).Key;
            //get the path of the latest backup
            var folderName = $"{repositoryName} {latestBackupKey:yyyy-MM-dd-HH-mm-ss}";
            var latestBackupPath = Path.Combine(backupFolderPath, folderName);
            //decrypt the latest backup
            _encryptionService.DecryptFolderAsync(latestBackupPath, token);

            var RepositoryAlreadyExistsOnGithub =
                _gitService.GetAllRepositoriesNamesAsync().Result.Contains(folderName.Replace(" ", "_"));

            if (RepositoryAlreadyExistsOnGithub)
            {
                Console.WriteLine(
                    "Latest backup already exists on Github. Update current one by creating a new backup.");
                return;
            }

            _gitService.UploadRemoteRepositoryAsync(folderName.Replace(" ", "_"));
            Console.WriteLine("Created remote repository successfully!");
            _encryptionService.EncryptFolderAsync(latestBackupPath, token);
        }
        else
        {
            Console.WriteLine("Repository does not exist. Please try again.");
        }
    }

    #endregion
}