using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.Interfaces;

/// <summary>
/// Defines data access operations for student records.
/// </summary>
public interface IStudentRepository
{
    /// <summary>Returns all stored students.</summary>
    List<Student> GetAll();

    /// <summary>Finds a student by ID, or returns null if none is found.</summary>
    Student? GetById(int studentId);

    /// <summary>Adds a student. Returns false if the ID already exists.</summary>
    bool Add(Student student);

    /// <summary>Updates a student. Returns false if the ID is not found.</summary>
    bool Update(Student student);

    /// <summary>Deletes a student. Returns false if the ID is not found.</summary>
    bool Delete(int studentId);
}