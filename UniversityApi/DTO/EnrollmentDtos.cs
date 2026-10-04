using System.ComponentModel.DataAnnotations;

namespace UniversityApi.DTO;

public class EnrollmentDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public int? Grade { get; set; }
}

public class EnrollmentCreateDto
{
    [Required]
    public int StudentId { get; set; }

    [Required]
    public int CourseId { get; set; }
}

public class EnrollmentUpdateGradeDto
{
    [Range(0, 100)]
    public int Grade { get; set; }
}