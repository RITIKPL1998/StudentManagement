using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.ConsoleApp.Interfaces;
using StudentManagement.ConsoleApp.Models;
using StudentManagement.ConsoleApp.Repositories;
using StudentManagement.ConsoleApp.Services;

var services = new ServiceCollection();
services.AddSingleton<IStudentRepository, StudentRepository>();
services.AddSingleton<IStudentService, StudentService>();

using var serviceProvider = services.BuildServiceProvider();
var studentService = serviceProvider.GetRequiredService<IStudentService>();

var continueUsing = true;

while (continueUsing)
{
    DisplayMenu();
    Console.Write("Select an option: ");
    var choice = Console.ReadLine()?.Trim();

    if (choice == "0")
    {
        break;
    }

    try
    {
        switch (choice)
        {
            case "1":
                AddStudent();
                break;
            case "2":
                DisplayStudents();
                break;
            case "3":
                SearchStudent();
                break;
            case "4":
                UpdateStudent();
                break;
            case "5":
                DeleteStudent();
                break;
            case "6":
                DisplayApplicationInformation();
                break;
            default:
                Console.WriteLine("Invalid option. Please select a number from 0 to 6.");
                break;
        }
    }
    catch (ValidationException ex)
    {
        Console.WriteLine($"Validation error: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }

    continueUsing = AskWhetherToContinue();
}

Console.WriteLine("Thank you for using the Student Management System.");

void DisplayMenu()
{
    Console.WriteLine("\n=== Student Management System ===");
    Console.WriteLine("1. Register a student");
    Console.WriteLine("2. Display all students");
    Console.WriteLine("3. Search for a student");
    Console.WriteLine("4. Update a student");
    Console.WriteLine("5. Delete a student");
    Console.WriteLine("6. Display Application Information");
    Console.WriteLine("0. Exit");
}

void AddStudent()
{
    Console.WriteLine("\n--- Register Student ---");

    var student = new Student
    {
        StudentId = ReadInt("Student ID: "),
        FirstName = ReadRequiredString("First name: "),
        LastName = ReadRequiredString("Last name: "),
        Email = ReadRequiredString("Email: "),
        Age = ReadInt("Age: "),
        Course = ReadRequiredString("Course: ")
    };

    Console.WriteLine(studentService.AddStudent(student)
        ? "Student registered successfully."
        : "A student with that ID already exists.");
}

void DisplayStudents()
{
    Console.WriteLine("\n--- All Students ---");
    var students = studentService.GetAllStudents();

    if (students.Count == 0)
    {
        Console.WriteLine("No students are registered.");
        return;
    }

    foreach (var student in students)
    {
        DisplayStudent(student);
    }
}

void SearchStudent()
{
    Console.WriteLine("\n--- Search Student ---");
    var studentId = ReadInt("Student ID: ");
    var student = studentService.GetStudentById(studentId);

    if (student is null)
    {
        Console.WriteLine("Student not found.");
        return;
    }

    DisplayStudent(student);
}

void UpdateStudent()
{
    Console.WriteLine("\n--- Update Student ---");
    var studentId = ReadInt("Student ID to update: ");
    var existingStudent = studentService.GetStudentById(studentId);

    if (existingStudent is null)
    {
        Console.WriteLine("Student not found.");
        return;
    }

    Console.WriteLine("Press Enter to keep the current value.");

    var updatedStudent = new Student
    {
        StudentId = existingStudent.StudentId,
        FirstName = ReadOptionalString("First name", existingStudent.FirstName),
        LastName = ReadOptionalString("Last name", existingStudent.LastName),
        Email = ReadOptionalString("Email", existingStudent.Email),
        Age = ReadOptionalInt("Age", existingStudent.Age),
        Course = ReadOptionalString("Course", existingStudent.Course)
    };

    Console.WriteLine(studentService.UpdateStudent(updatedStudent)
        ? "Student updated successfully."
        : "Student could not be updated.");
}

void DeleteStudent()
{
    Console.WriteLine("\n--- Delete Student ---");
    var studentId = ReadInt("Student ID to delete: ");
    var student = studentService.GetStudentById(studentId);

    if (student is null)
    {
        Console.WriteLine("Student not found.");
        return;
    }

    DisplayStudent(student);

    if (!ReadYesNo("Delete this student? (Y/N): "))
    {
        Console.WriteLine("Deletion cancelled.");
        return;
    }

    Console.WriteLine(studentService.DeleteStudent(studentId)
        ? "Student deleted successfully."
        : "Student could not be deleted.");
}

void DisplayStudent(Student student)
{
    Console.WriteLine(
        $"ID: {student.StudentId} | Name: {student.FirstName} {student.LastName} | " +
        $"Email: {student.Email} | Age: {student.Age} | Course: {student.Course}");
}

int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out var value))
        {
            return value;
        }

        Console.WriteLine("Please enter a valid whole number.");
    }
}

string ReadRequiredString(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var value = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(value))
        {
            return value.Trim();
        }

        Console.WriteLine("This value is required.");
    }
}

string ReadOptionalString(string fieldName, string currentValue)
{
    Console.Write($"{fieldName} [{currentValue}]: ");
    var value = Console.ReadLine()?.Trim();
    return string.IsNullOrEmpty(value) ? currentValue : value;
}

int ReadOptionalInt(string fieldName, int currentValue)
{
    while (true)
    {
        Console.Write($"{fieldName} [{currentValue}]: ");
        var input = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(input))
        {
            return currentValue;
        }

        if (int.TryParse(input, out var value))
        {
            return value;
        }

        Console.WriteLine("Please enter a valid whole number or press Enter to keep the current value.");
    }
}

bool ReadYesNo(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var answer = Console.ReadLine()?.Trim();

        if (string.Equals(answer, "Y", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (string.Equals(answer, "N", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        Console.WriteLine("Enter Y or N.");
    }
}

bool AskWhetherToContinue()
{
    return ReadYesNo("\nWould you like to continue? (Y/N): ");
}

void DisplayApplicationInformation()
{
    var version = System.Reflection.Assembly
        .GetExecutingAssembly()
        .GetName()
        .Version?.ToString() ?? "Version unavailable";

    Console.WriteLine("\n--- Application Information ---");
    Console.WriteLine("Application Name: Student Management Console Application");
    Console.WriteLine($"Application Version: {version}");
    Console.WriteLine("Description: A console application for managing student records.");
}