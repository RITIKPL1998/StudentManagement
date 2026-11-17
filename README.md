# Student Management System

A .NET 10 console application for managing student records. The project is organized to separate console interaction, student services, data access, and student models.

## Project Overview

The Student Management System provides a command-line interface for common student-record tasks. It is intended to help users maintain student information and retain records between application runs.

## Business Objectives

- Centralize student record management.
- Make student information easier to find and maintain.
- Improve data quality through validation and unique student IDs.
- Preserve student records between application sessions.
- Provide a foundation for future administrative features.

## Technology Stack

- **Language:** C#
- **Framework:** .NET 10
- **Application type:** Console application
- **Data storage:** JSON file (`Data/students.json`)

## Application Features

The application is structured to support:

- Adding and displaying student records.
- Searching for a student by ID.
- Updating and deleting student records.
- Validating student information and preventing duplicate IDs.
- Persisting student data in JSON format.
- Navigating operations through a console menu.

## Project Structure

```text
StudentManagementConsoleApplication/
├── .github/                 # Repository-specific guidance
├── Data/
│   └── students.json        # Student data file
├── docs/
│   ├── ARCHITECTURE.md      # Architecture documentation
│   ├── DEVELOPMENT.PLAN.md # Development plan
│   ├── REQUIREMENTS.md      # Functional and non-functional requirements
│   └── TEST-PLAN.md         # Test plan
├── Interfaces/              # Service and repository interfaces
├── Models/                  # Student data models
├── Presentations/           # Console menu and presentation
├── repositories/            # Data access implementations
├── Services/                # Student-management operations
├── Program.cs               # Application entry point
└── StudentManagementConsoleApplication.csproj
```

## Prerequisites

- .NET 10 SDK
- Windows, macOS, or Linux
- A terminal or command prompt

Check your installed .NET SDK:

```powershell
dotnet --version
```

## Build the Application

Open a terminal in the project directory and run:

```powershell
dotnet build
```

## Run the Application

From the project directory, run:

```powershell
dotnet run
```

## Future Enhancements

- Add automated unit and integration tests.
- Support importing and exporting student records.
- Add filtering, sorting, and reporting.
- Maintain an audit history of record changes.
- Add backup and recovery options.
- Introduce authentication and role-based access.