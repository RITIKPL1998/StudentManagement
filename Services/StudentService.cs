using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using StudentManagement.ConsoleApp.Interfaces;
using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.Services;

/// <summary>
/// Provides student-management operations and applies student validation rules.
/// </summary>
public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="StudentService"/> class.
    /// </summary>
    /// <param name="studentRepository">Repository used to access student records.</param>
    public StudentService(IStudentRepository studentRepository)
    {
        ArgumentNullException.ThrowIfNull(studentRepository);
        _studentRepository = studentRepository;
    }

    /// <summary>
    /// Validates and adds a student unless the student ID already exists.
    /// </summary>
    /// <param name="student">The student to add.</param>
    /// <returns><see langword="true"/> if added; otherwise, <see langword="false"/> for a duplicate ID.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="student"/> is null.</exception>
    /// <exception cref="ValidationException">The student data is invalid.</exception>
    public bool AddStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        ValidateStudent(student);

        if (_studentRepository.GetById(student.StudentId) is not null)
        {
            return false;
        }

        return _studentRepository.Add(student);
    }

    /// <summary>
    /// Gets all students.
    /// </summary>
    /// <returns>A list of student records.</returns>
    public List<Student> GetAllStudents() => _studentRepository.GetAll();

    /// <summary>
    /// Finds a student by ID.
    /// </summary>
    /// <param name="studentId">The student ID to search for.</param>
    /// <returns>The matching student, or <see langword="null"/> if not found.</returns>
    public Student? GetStudentById(int studentId) =>
        _studentRepository.GetById(studentId);

    /// <summary>
    /// Validates and updates an existing student.
    /// </summary>
    /// <param name="student">The student details to update.</param>
    /// <returns><see langword="true"/> if updated; otherwise, <see langword="false"/> if not found.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="student"/> is null.</exception>
    /// <exception cref="ValidationException">The student data is invalid.</exception>
    public bool UpdateStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        ValidateStudent(student);

        if (_studentRepository.GetById(student.StudentId) is null)
        {
            return false;
        }

        return _studentRepository.Update(student);
    }

    /// <summary>
    /// Deletes a student by ID.
    /// </summary>
    /// <param name="studentId">The ID of the student to delete.</param>
    /// <returns><see langword="true"/> if deleted; otherwise, <see langword="false"/>.</returns>
    public bool DeleteStudent(int studentId) =>
        _studentRepository.Delete(studentId);

    private static void ValidateStudent(Student student)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(student);

        if (!Validator.TryValidateObject(student, context, results, validateAllProperties: true))
        {
            string message = string.Join(
                Environment.NewLine,
                results.Select(result => result.ErrorMessage));

            throw new ValidationException(message);
        }
    }
}