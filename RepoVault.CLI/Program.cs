// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using RepoVault.Application.Git;
using RepoVault.Infrastructure.Services;

Console.WriteLine("Paste your user token here: ");
string token = Console.ReadLine();

GitServices gitServices = new();

if(gitServices.UserIsAuthenticated(token))
{
    Console.WriteLine("Welcome, you are logged in as "+gitServices.GetAuthenticatedUserLogin(token).Result);
}
else
{
    Console.WriteLine("You are not authenticated!");
    return;
}

Console.WriteLine("Here are your repositories: ");
var listnum = 1;
foreach(var repo in gitServices.ShowAllReposNames(token).Result)
{
    Console.WriteLine(listnum+". "+repo);
    listnum++;
}

Console.WriteLine("Enter the number of the repository you want to see the issues for: ");
int repoNum = Convert.ToInt32(Console.ReadLine());


