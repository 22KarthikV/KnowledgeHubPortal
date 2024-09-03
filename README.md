# Knowledge Hub Portal
[Portal -Hosted Live on Azure](https://knowledgehp.azurewebsites.net/)

Knowledge Hub Portal is a web application that allows users to submit, browse, and manage URLs under specific categories. It provides a platform for sharing knowledge and resources within a community.

## Features

- User Registration and Authentication
- URL Submission and Management
- Category-based Organization
- Admin Approval System
- Browse Approved URLs
- Responsive Design

## Technologies Used

- ASP.NET Core MVC
- Entity Framework Core
- C#
- HTML/CSS
- Bootstrap
- SQL Server

## Getting Started

### Prerequisites

- .NET 8.0
- SQL Server
- Visual Studio 2022 or Visual Studio Code

### Installation

1. Clone the repository:
`git clone https://github.com/22KarthikV/KnowledgeHubPortal.git`

2. Navigate to the project directory:
`cd KnowledgeHubPortal`

3. Update the connection string in `appsettings.json` to point to your SQL Server instance.

4. Run the following commands to set up the database:
`dotnet ef database update`

5. Run the application:
`dotnet run`

6. Open a web browser and navigate to `https://localhost:5001` (or the port specified in your launchSettings.json).

## Project Structure

- `KnowledgeHubPortal.Core`: Contains domain entities and interfaces.
- `KnowledgeHubPortal.Infrastructure`: Implements data access and services.
- `KnowledgeHubPortal.Web`: The main web application project.

## Usage

1. Register a new account or log in if you already have one.
2. Submit URLs along with their titles, descriptions, and categories.
3. Admins can approve or reject submitted URLs.
4. Browse approved URLs by category.

## Contributing

Contributions to the Knowledge Hub Portal are welcome. Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Contact

Karthik V - [GitHub Profile](https://github.com/22KarthikV)

Project Link: [https://github.com/22KarthikV/KnowledgeHubPortal](https://github.com/22KarthikV/KnowledgeHubPortal)
