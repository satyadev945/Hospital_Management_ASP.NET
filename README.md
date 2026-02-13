# Clinic Management System - .NET 8 Migration

This project has been migrated from ASP.NET Web Forms 4.5.2 to .NET 8 using clean architecture principles.

## Project Structure

```
/
├── src/
│   ├── ClinicManagement.Domain/          # Domain layer with entities and interfaces
│   ├── ClinicManagement.Application/     # Application layer with business logic
│   ├── ClinicManagement.Infrastructure/  # Infrastructure layer with data access
│   └── ClinicManagement.Web/             # Web layer with Razor Pages
├── tests/
│   ├── ClinicManagement.UnitTests/       # Unit tests
│   └── ClinicManagement.IntegrationTests/ # Integration tests
└── docs/                                  # Documentation
```

## Features

- Clean Architecture with separation of concerns
- Entity Framework Core 8.0 for data access
- ASP.NET Core Razor Pages for UI
- Dependency Injection throughout
- Serilog for logging
- Session-based authentication
- Health check endpoints

## Prerequisites

- .NET 8 SDK
- SQL Server (or update connection string in appsettings.json)

## Setup

1. Update database connection string in src/ClinicManagement.Web/appsettings.json or set environment variables:
   - DB_SERVER
   - DB_NAME
   - DB_USER
   - DB_PASSWORD

2. Build and run:
   ```bash
   dotnet build
   dotnet run --project src/ClinicManagement.Web
   ```

## Running Tests

```bash
dotnet test
```

## Key Migration Changes

### From Web Forms to Razor Pages
- SignUp.aspx → Pages/Account/Login.cshtml and Register.cshtml
- Patient pages → Pages/Patient/
- Doctor pages → Pages/Doctor/
- Admin pages → Pages/Admin/

### From ADO.NET to EF Core
- Direct SqlConnection → DbContext with repositories
- Stored procedures → LINQ queries
- DataTable/DataSet → Strongly-typed entities

### Configuration
- Web.config → appsettings.json
- Environment variable support maintained

### Session Management
- Session state migrated to ASP.NET Core session middleware
- Cookie-based session storage

## Health Endpoints

- /health - Returns "Healthy" when application is running
- /ready - Returns "Ready" when application is ready to serve requests

## Architecture

### Domain Layer
- Entities (User, Patient, Doctor, Appointment, Bill)
- Repository interfaces
- Service interfaces
- Domain exceptions

### Application Layer
- Service implementations
- DTOs
- AutoMapper profiles
- Business logic

### Infrastructure Layer
- EF Core DbContext
- Repository implementations
- Entity configurations
- Data access logic

### Web Layer
- Razor Pages
- ViewModels
- Dependency injection setup
- Static files
