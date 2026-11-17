using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.Interfaces;

/// <summary>
/// Defines student-management operations.
/// </summary>
public interface IStudentService
{
    bool AddStudent(Student student);
    List<Student> GetAllStudents();
    Student? GetStudentById(int studentId);
    bool UpdateStudent(Student student);
    bool DeleteStudent(int studentId);
}