# Student Management System Requirements

## 1. Purpose

This document defines the functional and non-functional requirements for the Student Management System, a .NET 10 console application. It is intended to guide implementation, testing, and future enhancements.

## 2. Scope

The system manages student records through a console menu. It supports creating, viewing, searching, updating, and deleting records. Student data is persisted in the JSON file `Data/students.json`.

## 3. Assumptions

- Each student has a unique student ID.
- Student fields and their formats are defined by the application’s student model.
- The initial storage mechanism is a JSON file.
- The application is intended for interactive use by one user at a time.
- The system reports storage errors and does not claim an operation succeeded unless the change is saved.

## 4. Functional Requirements

### FR-001 — Add a New Student

The system shall allow a user to add a student by entering the required student information.

**Acceptance criteria:**
- The system prompts for each required field.
- Submitted information is validated before saving.
- A valid record is saved to persistent storage.
- The system confirms successful creation.
- Invalid information or a duplicate student ID prevents creation and produces a clear message.

### FR-002 — Display All Students

The system shall allow a user to view all student records currently stored.

**Acceptance criteria:**
- Each record displays its student ID and available details.
- Records are presented in a consistent, readable format.
- If no records exist, the system clearly informs the user.
- A storage error is reported without presenting incomplete data as a successful result.

### FR-003 — Search for a Student by ID

The system shall allow a user to search for a student using the student ID.

**Acceptance criteria:**
- The system prompts for and validates the ID.
- If a matching record exists, its details are displayed.
- If no matching record exists, the system reports that no student was found.
- Search input is handled without terminating the application unexpectedly.

### FR-004 — Update Student Details

The system shall allow a user to update the details of an existing student identified by ID.

**Acceptance criteria:**
- The system locates the record using its student ID.
- If no matching record exists, the system reports this and makes no changes.
- Updated information is validated before saving.
- An update cannot assign an ID already used by another student.
- The system confirms success only after the updated record is persisted.

### FR-005 — Delete a Student

The system shall allow a user to delete an existing student by ID.

**Acceptance criteria:**
- The system locates the record using its student ID.
- If no matching record exists, the system reports this.
- The system requests confirmation before deletion.
- Confirmed deletion is persisted and acknowledged.
- Cancelling the operation leaves the record unchanged.

### FR-006 — Validate Student Information

The system shall validate student information when a record is added or updated.

**Acceptance criteria:**
- Required fields must not be empty or whitespace-only.
- Fields must satisfy the format and length rules defined by the application.
- Invalid values are rejected before persistence.
- Validation messages identify the field or issue and explain what the user should correct.

### FR-007 — Prevent Duplicate Student IDs

The system shall ensure that each student ID belongs to no more than one student record.

**Acceptance criteria:**
- The system checks existing records before adding a student.
- The system rejects an ID already assigned to another student.
- Updates preserve ID uniqueness.
- ID comparison and normalization rules are consistent across add, search, and update operations.

## 5. Non-Functional Requirements

### NFR-001 — Maintainable Code

The application shall be organized to support future changes and fixes.

**Acceptance criteria:**
- Components have clear responsibilities and meaningful names.
- Business rules are implemented consistently and are not unnecessarily duplicated.
- Documentation is updated when important behavior or design decisions change.

### NFR-002 — Proper Exception Handling

The application shall handle expected operational errors without unnecessary termination.

**Acceptance criteria:**
- Errors from user input, application operations, and JSON storage are handled appropriately.
- User-facing messages are clear and do not expose stack traces or implementation details.
- Recoverable errors allow the user to retry or return to the menu where practical.
- Failed storage operations are not reported as successful.

### NFR-003 — Input Validation

The application shall validate user input before using it in an operation.

**Acceptance criteria:**
- Menu selections are checked against available options.
- Required values and field-specific constraints are checked.
- Invalid input produces an actionable message and allows the user to continue where practical.
- Invalid input does not cause unhandled exceptions or unintended data changes.

### NFR-004 — Separation of Concerns

The application shall keep console presentation, student-management operations, data models, and persistence responsibilities appropriately separated.

**Acceptance criteria:**
- Console presentation is not responsible for implementing JSON storage details.
- Student-management rules can be changed without unnecessarily changing console formatting.
- Persistence access is handled through the repository/data-access boundary established by the project.

### NFR-005 — Readable Console Output

The application shall provide clear and consistent console prompts, menus, records, and operation results.

**Acceptance criteria:**
- Each prompt explains what input is expected.
- Success, validation, not-found, and storage-error messages are distinguishable.
- Student details are labeled consistently.
- Color is not the only means of communicating information.

### NFR-006 — Persistent Data Storage

The application shall retain student records between application runs using the configured JSON storage.

**Acceptance criteria:**
- Existing records are loaded when the application starts.
- Successful additions, updates, and deletions are reflected in `Data/students.json`.
- The application handles missing, unreadable, or invalid JSON data safely and reports the issue clearly.
- A failed write does not silently discard existing data or claim that a change was saved.

## 6. Requirements Summary

| ID | Requirement |
|---|---|
| FR-001 | Add a new student |
| FR-002 | Display all students |
| FR-003 | Search for a student by ID |
| FR-004 | Update student details |
| FR-005 | Delete a student |
| FR-006 | Validate student information |
| FR-007 | Prevent duplicate student IDs |
| NFR-001 | Maintainable code |
| NFR-002 | Proper exception handling |
| NFR-003 | Input validation |
| NFR-004 | Separation of concerns |
| NFR-005 | Readable console output |
| NFR-006 | Persistent data storage |