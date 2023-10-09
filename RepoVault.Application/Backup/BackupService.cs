using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RepoVault.Application.Encryption;
using RepoVault.Application.Git;

namespace RepoVault.Application.Backup;

public class BackupService : IBackupService
{
    private readonly string backupFolderPath;
    private readonly IGitService _gitService;
    private readonly IEncryptionService _encryptionService;

    public BackupService(string backupBasePath, IGitService gitService, IEncryptionService encryptionService)
    {
        backupFolderPath = Path.Combine(backupBasePath, "RepoVaultBackups");
        _gitService = gitService;
        _encryptionService = encryptionService;
    }
    
    public void CreateBackupFolder(string repoName, out string repoBackupFolderPath)
    {
        if (!Directory.Exists(backupFolderPath))
        {
            Directory.CreateDirectory(backupFolderPath);
        }

        repoBackupFolderPath = Path.Combine(backupFolderPath, $"{repoName} {DateTime.Now:yyyy-MM-dd-HH-mm-ss}");
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

    public void CreateBackupIssuesFile(string repoName, string repoBackupFolderPath)
    {
        var issuesData = _gitService.GetAllIssuesForRepository(repoName).Result;

        foreach (var issue in issuesData)
        {
            string json = JsonConvert.SerializeObject(issue);
            File.WriteAllText(Path.Combine(repoBackupFolderPath, $"{issue.Title}.json"), json);
        }
    }
    
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
            string[] directoryPaths = Directory.GetDirectories(backupFolderPath);

            // Create a dictionary to store the results
            var directoryDictionary = new Dictionary<DateTime, string>();

            // Specify the format of the date in the directory name
            string dateFormat = "yyyy-MM-dd-HH-mm-ss";

            // Iterate through directory paths
            foreach (string dirPath in directoryPaths)
            {
                // Extract the directory name without the full path
                string directoryName = Path.GetFileName(dirPath);

                // Split the directory name into parts using space as the separator
                string[] parts = directoryName.Split(' ');

                // Check if there are enough parts to extract a date and repository name
                if (parts.Length >= 2)
                {
                    // Attempt to parse the date part to DateTime using the specified format
                    if (DateTime.TryParseExact(parts[1], dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                    {
                        directoryDictionary[date] = parts[0];
                    }
                }
            }

            return directoryDictionary;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }

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
            _encryptionService.DecryptFolder(latestBackupPath, token);

            var RepositoryAlreadyExistsOnGithub =  _gitService.GetAllRepositoriesNames().Result.Contains(folderName.Replace(" ","_"));

            if (RepositoryAlreadyExistsOnGithub)
            {
                Console.WriteLine("Latest backup already exists on Github. Update current one by creating a new backup.");
                return;
            }
            _gitService.UploadRemoteRepository(folderName.Replace(" ","_"));
            Console.WriteLine("Created remote repository successfully!");
        }
        else
        {
            Console.WriteLine("Repository does not exist. Please try again.");
        }
    }
}