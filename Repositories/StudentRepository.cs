using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using StudentManagement.ConsoleApp.Interfaces;
using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.Repositories;

/// <summary>
/// Stores student records in a JSON file.
/// </summary>
public class StudentRepository : IStudentRepository
{
    private readonly string _filePath;
    private readonly List<Student> _students;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    /// <summary>
    /// Initializes the repository and loads student records from JSON storage.
    /// </summary>
    public StudentRepository()
    {
        _filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Data",
            "students.json");

        _students = LoadStudents();
    }

    /// <summary>
    /// Adds a student if its ID is not already stored.
    /// </summary>
    /// <param name="student">The student to add.</param>
    /// <returns>True if added; otherwise, false if the ID already exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="student"/> is null.</exception>
    public bool Add(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);

        if (_students.Any(existing => existing.StudentId == student.StudentId))
        {
            return false;
        }

        _students.Add(student);
        SaveStudents();
        return true;
    }

    /// <summary>
    /// Gets a copy of the stored student collection.
    /// </summary>
    /// <returns>A new list containing the stored students.</returns>
    public List<Student> GetAll() => new(_students);

    /// <summary>
    /// Finds a student by ID.
    /// </summary>
    /// <param name="studentId">The student ID to search for.</param>
    /// <returns>The matching student, or null if not found.</returns>
    public Student? GetById(int studentId) =>
        _students.FirstOrDefault(student => student.StudentId == studentId);

    /// <summary>
    /// Updates the stored student with the same ID.
    /// </summary>
    /// <param name="student">The student information to save.</param>
    /// <returns>True if updated; otherwise, false if the ID was not found.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="student"/> is null.</exception>
    public bool Update(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);

        int index = _students.FindIndex(
            existing => existing.StudentId == student.StudentId);

        if (index < 0)
        {
            return false;
        }

        _students[index] = student;
        SaveStudents();
        return true;
    }

    /// <summary>
    /// Deletes a student by ID.
    /// </summary>
    /// <param name="studentId">The ID of the student to delete.</param>
    /// <returns>True if deleted; otherwise, false if the ID was not found.</returns>
    public bool Delete(int studentId)
    {
        int index = _students.FindIndex(
            student => student.StudentId == studentId);

        if (index < 0)
        {
            return false;
        }

        _students.RemoveAt(index);
        SaveStudents();
        return true;
    }

    private List<Student> LoadStudents()
    {
        string? directory = Path.GetDirectoryName(_filePath);

        if (directory is not null)
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(_filePath))
        {
            File.WriteAllText(
                _filePath,
                JsonSerializer.Serialize(new List<Student>(), JsonOptions));

            return new List<Student>();
        }

        string json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Student>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<Student>>(json)
                ?? throw new InvalidDataException(
                    $"The JSON file '{_filePath}' contains null instead of a student list.");
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException(
                $"Could not read student records from '{_filePath}'. The file was not overwritten.",
                ex);
        }
    }

    private void SaveStudents()
    {
        string? directory = Path.GetDirectoryName(_filePath);

        if (directory is not null)
        {
            Directory.CreateDirectory(directory);
        }

        string temporaryPath = $"{_filePath}.{Guid.NewGuid():N}.tmp";
        string json = JsonSerializer.Serialize(_students, JsonOptions);

        try
        {
            File.WriteAllText(temporaryPath, json);
            File.Move(temporaryPath, _filePath, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
    }
}