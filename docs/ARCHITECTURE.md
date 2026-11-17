# Student Management System Architecture

## 1. Project Architecture Overview

The Student Management System is a .NET 10 console application organized into four layers:

1. **Presentation Layer** — Handles console menus, prompts, and output.
2. **Business Logic Layer** — Implements student-management operations and business rules.
3. **Data Access Layer** — Provides an implementation for storing and retrieving student records.
4. **Data Storage Layer** — Persists records in `Data/students.json`.

Each layer should depend on the layer below it through clear contracts. The presentation layer should not access JSON files directly, and business rules should not depend on console input or output.

## 2. Layer-Wise Responsibilities

### Presentation Layer

- Display the application menu and student information.
- Collect and perform basic validation of console input.
- Call business operations through `IStudentService`.
- Present success, validation, not-found, and error messages.
- Avoid implementing business rules or persistence.

### Business Logic Layer

- Implement adding, listing, searching, updating, and deleting students.
- Enforce student information rules and ID uniqueness.
- Coordinate operations with the data access layer through `IStudentRepository`.
- Return operation results that the presentation layer can communicate to the user.

### Data Access Layer

- Implement `IStudentRepository`.
- Load and save student records.
- Convert records between application models and JSON storage.
- Handle file and serialization errors and report them to the calling layer.
- Avoid making business decisions such as whether student details are valid.

### Data Storage Layer

- Store student records in `Data/students.json`.
- Retain records between application runs.
- Contain data only; it should not implement application behavior.

## 3. Component Descriptions

- **`Program.cs`** — Application entry point and composition root. Sets up dependencies and starts the console application.
- **`Presentations/ConsoleMenu.cs`** — Displays options, gathers input, invokes `IStudentService`, and presents results.
- **`Interfaces/IStudentService.cs`** — Defines student operations offered to the presentation layer.
- **`Services/StudentService.cs`** — Implements student operations and business rules.
- **`Interfaces/IStudentRepository.cs`** — Defines data operations required by the business logic layer.
- **`Repositories/StudentRepository.cs`** — Implements repository operations using JSON persistence.
- **`Models/Student.cs`** — Represents a student record.
- **`Data/students.json`** — Stores persisted student records.

The repository folders may initially be empty. Add implementation classes as the application is developed.

## 4. Class Responsibility Matrix

| Component | Layer | Responsibility |
|---|---|---|
| `Program` | Composition root | Configures dependencies and starts the application |
| `ConsoleMenu` | Presentation | Reads user input and displays menus, records, and messages |
| `IStudentService` | Business contract | Exposes student-management operations to presentation |
| `StudentService` | Business logic | Applies validation, uniqueness rules, and use-case behavior |
| `IStudentRepository` | Data contract | Defines operations for retrieving and persisting records |
| `StudentRepository` | Data access | Implements record operations using JSON storage |
| `Student` | Model | Represents a student and its data |
| `students.json` | Data storage | Retains student records between runs |

## 5. Application Execution Flow

1. The application starts in `Program`.
2. `Program` configures the repository, service, and console menu dependencies.
3. The console menu displays available operations.
4. The user selects an operation and provides input.
5. The menu calls the appropriate operation through `IStudentService`.
6. The service applies business rules and calls `IStudentRepository` when data access is needed.
7. The repository reads or updates `Data/students.json`.
8. The result returns through the service to the menu.
9. The menu displays the result and continues until the user exits.

## 6. Student Registration Flow

1. `ConsoleMenu` prompts the user for student details.
2. The menu passes the details to `IStudentService`.
3. `StudentService` validates required fields and documented formats.
4. The service checks for an existing student with the same ID through `IStudentRepository`.
5. If validation fails or the ID already exists, the service returns an appropriate failure result.
6. If the details are valid and the ID is unique, the service asks the repository to save the student.
7. `StudentRepository` persists the updated records to `Data/students.json`.
8. The service reports success only after persistence succeeds.
9. The menu displays a success or failure message.

## 7. Data Storage Strategy

The initial storage strategy is a JSON file at `Data/students.json`.

- Load records when needed by repository operations or at application startup.
- Persist successful additions, updates, and deletions.
- Do not report a change as successful until the write completes.
- Handle a missing data file as an empty data store if that is the chosen initial-file behavior.
- Report malformed or inaccessible storage clearly; do not silently overwrite data that cannot be read.
- Keep storage path and serialization behavior inside the data access layer.
- Use temporary files or another safe-write approach if needed to reduce the risk of corrupting existing data during writes.

This simple file-based approach is suitable for the initial single-user application. A database can replace it later without changing the presentation layer or the service contract.

## 8. Dependency Injection Strategy

`Program` is the composition root: it is the one place responsible for assembling the application’s dependencies.

- `ConsoleMenu` receives `IStudentService`.
- `StudentService` receives `IStudentRepository`.
- `StudentRepository` uses the JSON storage location and serialization behavior.

Dependencies should be provided to constructors rather than created inside the classes that use them. This keeps components replaceable and testable.

For a small beginner project, dependencies can be constructed manually in `Program`. A dependency-injection container can be introduced if the project needs it; the application should not add container complexity without a clear benefit.

## 9. Error Handling Strategy

- Validate user input and return clear, actionable messages for invalid values.
- Distinguish validation failures, duplicate IDs, missing students, and storage failures.
- Catch file and JSON serialization errors at the data access boundary.
- Report safe, understandable messages to users; keep technical details out of normal console output.
- Do not suppress errors or claim an operation succeeded when persistence fails.
- Keep the application running after recoverable errors so the user can retry or return to the menu.
- Avoid broad exception handling that hides programming defects.

## 10. Unit Testing Strategy

Test each layer at the appropriate boundary:

- **Business logic tests:** Test `StudentService` using a fake or in-memory `IStudentRepository`. Cover valid operations, validation failures, duplicate IDs, missing records, and update behavior.
- **Repository tests:** Use a temporary JSON file to test loading and saving. Cover empty storage, normal CRUD operations, malformed JSON, and file-access failures.
- **Presentation tests:** Keep console interaction straightforward. If tested, make input and output replaceable or use focused tests for menu behavior.
- **Integration tests:** Optionally test the service and JSON repository together using temporary storage.

Tests must not read from or write to the real `Data/students.json`. Each test should use isolated data and clean up temporary files afterward.

## Mermaid Architecture Diagram

````mermaid
flowchart TD
    User[User] --> Presentation[Presentation Layer<br/>ConsoleMenu]
    Presentation -->|IStudentService| Business[Business Logic Layer<br/>StudentService]
    Business -->|IStudentRepository| Access[Data Access Layer<br/>StudentRepository]
    Access -->|Read and write JSON| Storage[Data Storage Layer<br/>Data/students.json]
    Program[Program<br/>Composition root] -. configures .-> Presentation
    Program -. configures .-> Business
    Program -. configures .-> Access