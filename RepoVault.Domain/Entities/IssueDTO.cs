namespace RepoVault.Domain.Entities;

public class IssueDto
{
    public IssueDto(string title, string body, string state, string createdAt, string updatedAt, string closedAt,
        string author, string url)
    {
        Title = title;
        Body = body;
        State = state;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        ClosedAt = closedAt;
        Author = author;
        Url = url;
    }

    public string Title { get; set; }
    public string Body { get; set; }
    public string State { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
    public string ClosedAt { get; set; }
    public string Author { get; set; }
    public string Url { get; set; }
}