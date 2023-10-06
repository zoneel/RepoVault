using RepoVault.CLI;
using RepoVault.Infrastructure.Services;
//pipeline
Console.WriteLine("Welcome to RepoVault!");

Console.WriteLine("Paste your user's token here: ");
string token = Console.ReadLine();
UserInteraction.checkUserToken(token, out GitServices gitServices);

Console.WriteLine("Here are your repositories: ");
UserInteraction.ShowUserRepositories(gitServices, token);

Console.WriteLine("Enter name of repository you want to see the issues for: ");
string repoName = Console.ReadLine();


Console.WriteLine("Here are the issues for "+repoName+": ");
await UserInteraction.ShowRepoIssues(gitServices, token, repoName);

