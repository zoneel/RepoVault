namespace RepoVault.Domain.Exceptions;

public class NullIssueException : Exception
{
    public NullIssueException(string message) : base(message)
    {
        
    }
}