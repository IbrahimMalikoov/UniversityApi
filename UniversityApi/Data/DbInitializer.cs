using UniversityApi.Models;

namespace UniversityApi.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Teachers.Any()) return;

        var teacher = new Teacher
        {
            FirstName = "Евгений",
            LastName = "Герцен",
            Email = "e.gertsen@satbayev.university",
            Department = "Программная инженерия"
        };
        context.Teachers.Add(teacher);
        context.SaveChanges();

        var course = new Course
        {
            Name = "Разработка веб-сервисов (CSE5032)",
            Description = "Курс по ASP.NET Core Web API, REST и EF Core",
            Credits = 5,
            TeacherId = teacher.Id
        };
        context.Courses.Add(course);
        context.SaveChanges();

        var student = new Student
        {
            FirstName = "Аян",
            LastName = "Сериков",
            Email = "ayan.serikov@satbayev.university",
            BirthDate = new DateTime(2003, 5, 14)
        };
        context.Students.Add(student);
        context.SaveChanges();

        var enrollment = new Enrollment
        {
            StudentId = student.Id,
            CourseId = course.Id,
            Grade = 95
        };
        context.Enrollments.Add(enrollment);
        context.SaveChanges();
    }
}