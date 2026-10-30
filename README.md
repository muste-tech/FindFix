# FindFix

FindFix is a web marketplace that connects customers with repair technicians. A customer searches for a service in their area, sends a job request, and leaves a review after the work is done. Technicians build a profile, accept or decline jobs, and track each job through to completion.

## Features

**Customers**
- Register and log in
- Edit profile (location)
- Search for technicians by service and location, ordered by weighted rating
- View technician profiles with qualifications
- Send job requests to a technician
- Track requests on My Requests
- Leave a 1 to 5 star review on a completed job (one review per job)

**Technicians**
- Register and log in
- Complete a profile: services offered, location, experience, description, and qualifications
- View incoming job requests
- Accept or decline pending jobs
- Start an accepted job, then mark it complete
- Browser notifications for job status changes while the site is open

**Account and security**
- ASP.NET Identity for accounts, passwords, and roles
- Forgot password without email: the user answers security questions built from their own account data
- Answers are checked on the server; the correct answers never reach the browser
- Anti-forgery tokens on all form submissions
- Account lockout after 3 wrong answers, stored in the database, for 30 minutes

## Tech Stack

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core with Pomelo/Microting MySQL provider
- MySQL 8
- ASP.NET Identity
- Razor views with Bootstrap
- Vanilla JavaScript for notifications

## Project Structure

```
FindFix/
├── Controllers/     Account, Customer, Technician, Admin, Notifications
├── Models/          Customer, Technician, JobRequest, Review, Service, SecurityQuestion
├── Services/        CustomerQuestionBuilder, TechnicianQuestionBuilder
├── Data/            AppDbContext
├── Views/           Razor pages per controller
└── wwwroot/js/      notifications.js
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- MySQL 8 server running locally
- Visual Studio 2022 or VS Code with the C# extension

### Setup

1. Clone the repository:
   ```
   git clone https://github.com/muste-tech/FindFix.git
   cd FindFix
   ```

2. Create an empty MySQL database:
   ```sql
   CREATE DATABASE findfix_db;
   ```

3. Store your database connection string in user secrets, so the password never goes into source control:
   ```
   cd FindFix
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=findfix_db;User=root;Password=YOUR_PASSWORD;"
   ```

4. Apply the database migrations:
   ```
   dotnet ef database update
   ```

5. Run the app:
   ```
   dotnet run
   ```
   Open `http://localhost:5168` in your browser.

### Demo Data

After setup, the database is empty. Register a technician and a customer, complete the technician's profile, send a job request, then accept, start, and complete the job as the technician. Log in as the customer to leave a review.

To enable job notifications, log in and click **Turn on job notifications**, then allow the browser prompt. Notifications work on `localhost`. On a live site they require HTTPS.

## Known Limitations

- **Password reset guessing:** restarting the reset flow no longer clears the wrong-answer count, since it's stored on the account. The remaining risk is that someone who knows the account owner can answer the questions. A one-time code sent to a phone or email is the planned stronger fix.
- **No email or SMS delivery yet.** Notifications only appear while the site is open in a browser. Closed-app push notifications need a push service, which is planned.
- **Customer location** is entered on the profile page, but customers are not asked for other details.
- **Search requires login.** Visitors who are not logged in are redirected to the login page.
- **Not deployed.** The app runs locally. Production use needs HTTPS and a secure secret store.

## Planned Next Steps

- Lockout-aware forgot password messages on the reset page
- Push notifications when the site is closed
- A WhatsApp contact link, and later an AI assistant that suggests a service from a customer's description
- Map view of the technician's location once a job is in progress
- Seasonal maintenance reminders and a technician spotlight

## Screenshots

Add screenshots to an `images/` folder and link them here, for example:

```markdown
![Search results](images/search.png)
![Technician requests](images/technician-requests.png)
![Leave a review](images/leave-review.png)
```