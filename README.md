# FindFix

FindFix is a web marketplace that connects customers with repair technicians.

## Features

- Customer and technician registration and login
- Technician profiles with services and qualifications
- Search for technicians by service and location
- Job requests with accept, start, and complete steps
- Reviews and ratings after a completed job
- Forgot password using security questions

## Tech Stack

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core
- MySQL
- ASP.NET Identity

## Setup

1. Create a MySQL database named `findfix_db`.
2. Set your connection string with user secrets:
   `dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=findfix_db;User=root;Password=YOUR_PASSWORD;"`
3. Apply the database migrations: `dotnet ef database update`
4. Run the app: `dotnet run`

## Known Limitations

- Password reset security questions can be guessed by restarting the flow. A database lockout is planned.
- No email or SMS delivery yet. Notifications are planned.