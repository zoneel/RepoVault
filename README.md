# RepoVault

RepoVault is a C# application built on clean architecture that leverages the OctoKit GitHub API client. This tool empowers users to create secure backups of repositories and issues hosted on GitHub while ensuring the confidentiality of sensitive data through AES encryption.
![overview](https://github.com/zoneel/RepoVault/assets/40122657/a187761a-37be-435e-8176-ec3140ee7c5f)

## Features

- **Repository Backup:** RepoVault allows users to create backups of their GitHub repositories. It retrieves all the necessary data, including code, issues, pull requests, and project documentation, ensuring that valuable project information is safeguarded.

- **Issue Backup:** The application provides a straightforward way to back up issues associated with a GitHub repository. This ensures that your team's discussions, bug reports, and feature requests are preserved.

- **GitHub Integration:** RepoVault seamlessly integrates with the GitHub API through OctoKit, simplifying the backup process. Users can authorize the application to access their GitHub repositories securely.

- **AES Encryption:** Security is a top priority. RepoVault encrypts all locally stored backup files using the AES (Advanced Encryption Standard) algorithm. This ensures that your sensitive data remains confidential and protected against unauthorized access.

- **Backup Posting:** Users have the option to post their backups directly to GitHub repositories. This feature streamlines the recovery process, making it easy to restore your data when needed.

## Architecture
RepoVault follows the Clean Architecture design pattern to ensure maintainability, scalability, and separation of concerns.

- **Domain Layer:** The heart of RepoVault, this layer defines the core business logic and entities. It encapsulates the essential rules and behaviors of the application, ensuring the independence of the domain from external dependencies.

- **Application Layer:** Responsible for orchestrating use cases and business operations, the application layer bridges the gap between the domain layer and external interfaces. It coordinates how data flows and operations are executed.

- **Infrastructure Layer:** This layer handles external concerns such as data storage, API interactions (including OctoKit for GitHub), and encryption. It provides implementation details for interacting with external systems while shielding the core domain from direct dependencies.

- **CLI Layer:** Serving as the user interface, the Command-Line Interface (CLI) layer interacts with users and translates their commands into actions within the application. It provides a user-friendly interface for performing backups and managing data.
![image](https://github.com/zoneel/RepoVault/assets/40122657/8f8d185d-b207-4790-aab7-47c4681102ab)


## Getting Started

To get started with RepoVault, follow these steps:

1. Clone the repository to your local machine.
2. Build the project using Visual Studio or your preferred C# development environment. (There might be an issue with behaviour of SQLite while in debugging mode, when encountered try putting .db file inside RepoVault.CLI folder)
4. Use the command-line interface to initiate backups and manage your data.

## License

This project is licensed under the [MIT License](LICENSE.md) - see the [LICENSE.md](LICENSE.md) file for details.
