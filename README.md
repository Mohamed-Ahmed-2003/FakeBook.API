# FakeBook API - README

Welcome to the FakeBook API, a social networking API built using .NET 8. This project follows the principles of Clean Architecture and utilizes various modern technologies such as Entity Framework Core, SQL, and SignalR for real-time messaging.

## Table of Contents

1. [Project Overview](#project-overview)
2. [Technologies Used](#technologies-used)
3. [Features](#features)
4. [Architecture](#architecture)
5. [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
    - [Configuration](#configuration)
6. [Running the Application](#running-the-application)
7. [API Versioning](#api-versioning)
8. [Contributing](#contributing)
9. [License](#license)

## Project Overview

The FakeBook API is a social networking service that allows users to connect, share content, and communicate in real time. The API is designed to be scalable and maintainable, adhering to the principles of Clean Architecture.

## Technologies Used

- **.NET 8**: The main framework for building the API.
- **Entity Framework Core**: ORM for database operations.
- **SQL**: Relational database for data storage.
- **SignalR**: For real-time messaging capabilities.
- **ASP.NET Core**: For building the web API.
- **AutoMapper**: For object-object mapping.
- **MediatR**: For implementing CQRS (Command Query Responsibility Segregation).
- **Swagger**: For API documentation.
- **XUnit**: For unit testing.

## Features

- **User Authentication and Authorization**: Register, login, and manage user roles.
- **Profile Management**: Create and manage user profiles.
- **Friendship Management**: Send, accept, and decline friend requests.
- **Posts and Comments**: Create, read, update, and delete posts and comments.
- **Likes and Reactions**: Like and react to posts and comments.
- **Real-Time Messaging**: Chat with friends in real time using SignalR.
- **Notifications**: Receive notifications for various activities.
- **Search**: Search for users, posts, and other content.
- **API Versioning**: Support for multiple API versions.

## Architecture

The project follows the principles of Clean Architecture, which separates concerns into different layers:

- **Core**: Contains business logic and domain entities.
- **Application**: Contains application services, DTOs, and interfaces.
- **Infrastructure**: Contains data access logic, external services, and implementations.
- **Presentation**: Contains the API controllers and the user interface (if any).

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server or any other relational database
- Docker (optional, for containerization)
- Visual Studio or any other C# IDE

### Installation

1. Clone the repository:

    ```sh
    git clone https://github.com/Mohamed-Ahmed-2003/fakebook-api.git
    cd fakebook-api
    ```

2. Install the required packages:

    ```sh
    dotnet restore
    ```

### Configuration

1. Set up the database connection string in `appsettings.json`:

    ```json
    {
        "ConnectionStrings": {
            "DefaultConnection": "Server=your_server;Database=FakeBookDb;User Id=your_username;Password=your_password;"
        }
    }
    ```

2. Apply database migrations:

    ```sh
    dotnet ef database update
    ```

## Running the Application

1. Start the API:

    ```sh
    dotnet run
    ```

2. Navigate to `https://localhost:5001/swagger` to view the API documentation.

## API Versioning

The API supports versioning to ensure backward compatibility as new features are added. The versioning strategy used is URL segment versioning. For example:

- `https://localhost:5001/api/v1/posts`
- `https://localhost:5001/api/v2/posts`

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request for review.

## License

This project is licensed under the MIT License.

---

Thank you for using the FakeBook API! If you have any questions or need further assistance, feel free to contact us.
