using System.ComponentModel.DataAnnotations;

namespace RepoVault.Infrastructure.Database.Models;

public class BackupLog
{
    public BackupLog(string repositoryName, string backupDate)
    {
        RepositoryName = repositoryName;
        BackupDate = backupDate;
    }

    [Key] public int Id { get; set; }

    public string RepositoryName { get; set; }
    public string BackupDate { get; set; }
}