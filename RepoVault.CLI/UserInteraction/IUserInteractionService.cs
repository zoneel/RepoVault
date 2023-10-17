using RepoVault.Infrastructure.Git;

namespace RepoVault.CLI.UserInteraction;

public interface IUserInteractionService
{
    // Show menu
    public void ShowMenu();

    // Choose an action
    public void ChooseAction(out string response);
    
    // Authenticate user
    public void AuthenticateUser(out string correctToken, out IGitRepository correctGitServices);
    
    // Show user repositories
    public void ShowUserRepositories(IGitRepository gitServices1, string s);
    
    // Show local backups
    public void ShowLocalBackups(string s);
    
    // Show all Issues that repository has
    public Task ShowRepoIssues(IGitRepository gitRepository, string token, string repoName);
    
    // Show styled response
    public void ShowStyledResponse(string text);

}