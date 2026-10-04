using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniversityApi.Common;
using UniversityApi.DTO;
using UniversityApi.Models;
using UniversityApi.Repositories;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IRepository<Enrollment> _enrollmentRepo;
    private readonly IRepository<Student> _studentRepo;
    private readonly IRepository<Course> _courseRepo;
    private readonly IMapper _mapper;

    public EnrollmentsController(
        IRepository<Enrollment> enrollmentRepo,
        IRepository<Student> studentRepo,
        IRepository<Course> courseRepo,
        IMapper mapper)
    {
        _enrollmentRepo = enrollmentRepo;
        _studentRepo = studentRepo;
        _courseRepo = courseRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ReturnResult<IEnumerable<EnrollmentDto>>>> GetAll()
    {
        var enrollments = await _enrollmentRepo.GetAllAsync();
        return Ok(ReturnResult<IEnumerable<EnrollmentDto>>.Success(_mapper.Map<IEnumerable<EnrollmentDto>>(enrollments)));
    }

    [HttpPost]
    public async Task<ActionResult<ReturnResult<EnrollmentDto>>> Enroll(EnrollmentCreateDto dto)
    {
        var studentExists = await _studentRepo.ExistsAsync(s => s.Id == dto.StudentId);
        if (!studentExists)
        {
            return NotFound(ReturnResult<EnrollmentDto>.Fail("STUDENT_NOT_FOUND", "Student does not exist", 404));
        }

        var courseExists = await _courseRepo.ExistsAsync(c => c.Id == dto.CourseId);
        if (!courseExists)
        {
            return NotFound(ReturnResult<EnrollmentDto>.Fail("COURSE_NOT_FOUND", "Course does not exist", 404));
        }

        var alreadyEnrolled = await _enrollmentRepo.ExistsAsync(e => e.StudentId == dto.StudentId && e.CourseId == dto.CourseId);
        if (alreadyEnrolled)
        {
            return Conflict(ReturnResult<EnrollmentDto>.Fail("ALREADY_ENROLLED", "Student is already enrolled in this course", 409));
        }

        var enrollment = _mapper.Map<Enrollment>(dto);
        await _enrollmentRepo.AddAsync(enrollment);

        return StatusCode(201, ReturnResult<EnrollmentDto>.Success(_mapper.Map<EnrollmentDto>(enrollment), 201));
    }

    [HttpPut("{id}/grade")]
    public async Task<ActionResult<ReturnResult<EnrollmentDto>>> UpdateGrade(int id, EnrollmentUpdateGradeDto dto)
    {
        var enrollment = await _enrollmentRepo.GetByIdAsync(id);
        if (enrollment == null)
        {
            return NotFound(ReturnResult<EnrollmentDto>.Fail("ENROLLMENT_NOT_FOUND", "Enrollment record not found", 404));
        }

        enrollment.Grade = dto.Grade;
        await _enrollmentRepo.UpdateAsync(enrollment);

        return Ok(ReturnResult<EnrollmentDto>.Success(_mapper.Map<EnrollmentDto>(enrollment)));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ReturnResult<string>>> Delete(int id)
    {
        var enrollment = await _enrollmentRepo.GetByIdAsync(id);
        if (enrollment == null)
        {
            return NotFound(ReturnResult<string>.Fail("ENROLLMENT_NOT_FOUND", "Enrollment record not found", 404));
        }

        await _enrollmentRepo.DeleteAsync(enrollment);
        return Ok(ReturnResult<string>.Success("Student removed from course successfully"));
    }
}