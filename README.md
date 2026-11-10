# Gym Management System

A web-based gym management application built with **ASP.NET Core MVC and .NET 10**. The system helps gym administrators manage members, trainers, membership plans, training sessions, bookings, and attendance through a layered architecture.

## Overview

Gym Management System is designed to bring the main day-to-day gym administration tasks into one application. It separates the MVC presentation layer, business logic, and data access responsibilities to keep the codebase organized and easier to maintain.

## Features

### Dashboard & Analytics
- View total members and trainers.
- Track active memberships.
- Monitor upcoming, ongoing, and completed sessions.

### Member Management
- Create, view, update, and delete member records.
- View member profiles and health records.
- Upload and retrieve member profile photos.
- Store member information, including address and personal details.

### Trainer Management
- Create, view, update, and delete trainer records.
- Manage trainer details, specialties, and contact information.

### Membership & Plan Management
- View membership plans and plan details.
- Update plans and activate or deactivate them.
- Assign plans to members and calculate membership end dates.
- View active memberships and cancel them.
- Prevent creating another active membership for a member.

### Session Management
- Create, view, update, and delete training sessions.
- Associate sessions with trainers and categories.
- Track session dates, capacity, and available slots.

### Booking & Attendance
- Create bookings for upcoming sessions.
- View members booked for upcoming and ongoing sessions.
- Cancel bookings.
- Record member attendance.

### Authentication & Authorization
- Sign in and sign out using **ASP.NET Core Identity**.
- Use role-based authorization for protected areas of the application.
- Seed the `SuperAdmin` and `Admin` roles during application startup.

## Architecture

The solution follows a **Layered Architecture** with three projects:

| Project | Responsibility |
| --- | --- |
| `GymManagement` | Presentation Layer: ASP.NET Core MVC controllers, Razor views, application configuration, and request handling. |
| `GymManagementSystem.BLL` | Business Logic Layer: application services, service interfaces, ViewModels, AutoMapper profile, and operation results. |
| `GymManagement.DAL` | Data Access Layer: EF Core DbContext, entities, entity configurations, repositories, Unit of Work, migrations, and data seeding. |

### Design Patterns & Practices

- **Repository Pattern:** separates data access operations from business logic.
- **Generic Repository:** provides reusable data access methods for common entity operations.
- **Unit of Work:** coordinates repositories and persists changes through a shared DbContext.
- **Dependency Injection:** registers repositories, services, and application dependencies.
- **Asynchronous Programming:** uses async database operations and cancellation tokens.
- **DTO/ViewModel Mapping:** uses AutoMapper to map between entities and ViewModels.
- **Entity Configuration:** applies EF Core configurations from the DAL assembly.

## Technology Stack

- **Framework:** .NET 10, ASP.NET Core MVC
- **Language:** C#
- **ORM:** Entity Framework Core 10
- **Database:** Microsoft SQL Server
- **Authentication:** ASP.NET Core Identity
- **Object Mapping:** AutoMapper
- **Frontend:** Razor Views, HTML, CSS, JavaScript
- **Development Tools:** Visual Studio, Git, GitHub

## Solution Structure

```text
GymManagmentSystem/
│
├── GymManagement/
│   ├── Controllers/
│   ├── Views/
│   ├── wwwroot/
│   ├── Program.cs
│   └── GymManagement.PL.csproj
│
├── GymManagementSystem.BLL/
│   ├── Common/
│   ├── Services/
│   │   ├── Attachment/
│   │   ├── Classes/
│   │   └── Interfaces/
│   ├── ViewModels/
│   ├── MappingProfile.cs
│   └── GymManagementSystem.BLL.csproj
│
├── GymManagement.DAL/
│   ├── Configurations/
│   ├── DataSeeding/
│   ├── Date/
│   │   └── Migrations/
│   ├── DbContexts/
│   ├── Models/
│   ├── Repositories/
│   │   ├── Classes/
│   │   └── Interfaces/
│   ├── UnitOfWork.cs
│   └── GymManagement.DAL.csproj
│
└── GymManagementSolutions.slnx
```

## Getting Started

### Prerequisites

Make sure you have the following installed:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Visual Studio 2026](https://visualstudio.microsoft.com/) or another compatible .NET IDE

### 1. Clone the repository

```bash
git clone https://github.com/HabibaElbehairy1/GymManagmentSystem.git
cd GymManagmentSystem
```

### 2. Configure the database connection

Configure the `DefaultConnection` connection string for your local SQL Server instance.

For local development, you can use .NET User Secrets instead of storing machine-specific settings in the repository:

```bash
dotnet user-secrets init --project GymManagement
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=YOUR_SERVER;Database=GymDb;Trusted_Connection=True;TrustServerCertificate=True;" --project GymManagement
```

Replace `YOUR_SERVER` with your SQL Server instance.

### 3. Configure the initial administrator

The application seeds Identity roles and initial administrator accounts during startup. Configure the required `AdminSeed` settings securely using User Secrets or environment variables before running the application:

- `AdminSeed:SuperAdminEmail`
- `AdminSeed:Password`
- `AdminSeed:AdminEmail`
- `AdminSeed:SuperAdminPhone`
- `AdminSeed:AdminPhone`

Example (use your own values):

```bash
dotnet user-secrets set "AdminSeed:SuperAdminEmail" "your-superadmin-email@example.com" --project GymManagement
dotnet user-secrets set "AdminSeed:Password" "YOUR_STRONG_PASSWORD" --project GymManagement
dotnet user-secrets set "AdminSeed:AdminEmail" "your-admin-email@example.com" --project GymManagement
dotnet user-secrets set "AdminSeed:SuperAdminPhone" "YOUR_PHONE_NUMBER" --project GymManagement
dotnet user-secrets set "AdminSeed:AdminPhone" "YOUR_ADMIN_PHONE_NUMBER" --project GymManagement
```

> **Security note:** Do not commit real credentials or connection strings. The current Identity seeding implementation contains fallback values in source code; replace those fallbacks with secure configuration and rotate any credentials that may have been exposed before deploying the application.

### 4. Apply database migrations

The application checks for pending EF Core migrations and applies them during startup. You can also update the database manually from the solution directory:

```bash
dotnet ef database update --project GymManagement.DAL --startup-project GymManagement
```

### 5. Build and run

```bash
dotnet build GymManagementSolutions.slnx
dotnet run --project GymManagement
```

Open the local URL displayed in the terminal.

## Notes

- The application uses SQL Server through the `DefaultConnection` connection string.
- Initial plan data is loaded from a JSON seed file.
- Identity roles and initial administrator accounts are seeded during startup.
- The application applies pending database migrations on startup.

## Author

**Habiba Elbehairy**

[GitHub Profile](https://github.com/HabibaElbehairy1)

[Project Repository](https://github.com/HabibaElbehairy1/GymManagmentSystem)
