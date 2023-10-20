namespace RepoVault.Domain.Entities;

public class GithubRepository
{
    public GithubRepository(long id, string name, string description, string language, DateTimeOffset createdAt,
        DateTimeOffset updatedAt, long size, long openIssuesCount)
    {
        Id = id;
        Name = name;
        Description = description;
        Language = language;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Size = size;
        OpenIssuesCount = openIssuesCount;
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Language { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public long Size { get; set; }
    public long OpenIssuesCount { get; set; }
}