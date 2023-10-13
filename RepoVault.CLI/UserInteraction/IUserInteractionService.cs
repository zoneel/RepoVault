using RepoVault.Infrastructure.Services;

namespace RepoVault.CLI.UserInteraction;

public interface IUserInteractionService
{
    // Show menu
    public void ShowMenu();

    // Choose an action
    public void ChooseAction(out string response);

    // Check if user token is valid
    public bool CheckUserToken(string token, out GitRepository gitRepository);
    
    // Authenticate user
    public void AuthenticateUser(out string correctToken, out GitRepository correctgitServices);
    
    // Show user repositories
    public void ShowUserRepositories(GitRepository gitServices1, string s);
    
    // Show local backups
    public void ShowLocalBackups(string s);
    
    // Show all Issues that repository has
    public Task ShowRepoIssues(GitRepository gitRepository, string token, string repoName);
    
    // Show styled response
    public void ShowStyledResponse(string text);

}