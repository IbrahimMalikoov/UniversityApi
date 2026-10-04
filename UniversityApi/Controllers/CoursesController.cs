using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniversityApi.Common;
using UniversityApi.DTO;
using UniversityApi.Models;
using UniversityApi.Repositories;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly IRepository<Course> _courseRepo;
    private readonly IRepository<Teacher> _teacherRepo;
    private readonly IMapper _mapper;

    public CoursesController(IRepository<Course> courseRepo, IRepository<Teacher> teacherRepo, IMapper mapper)
    {
        _courseRepo = courseRepo;
        _teacherRepo = teacherRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ReturnResult<IEnumerable<CourseDto>>>> GetAll([FromQuery] string? search)
    {
        var courses = string.IsNullOrEmpty(search)
            ? await _courseRepo.GetAllAsync()
            : await _courseRepo.FindAsync(c => c.Name.ToLower().Contains(search.ToLower()));

        return Ok(ReturnResult<IEnumerable<CourseDto>>.Success(_mapper.Map<IEnumerable<CourseDto>>(courses)));
    }

    [HttpPost]
    public async Task<ActionResult<ReturnResult<CourseDto>>> Create(CourseCreateDto dto)
    {
        var teacherExists = await _teacherRepo.ExistsAsync(t => t.Id == dto.TeacherId);
        if (!teacherExists)
        {
            return BadRequest(ReturnResult<CourseDto>.Fail("TEACHER_NOT_FOUND", "Referenced teacher does not exist", 400));
        }

        var course = _mapper.Map<Course>(dto);
        await _courseRepo.AddAsync(course);
        return StatusCode(201, ReturnResult<CourseDto>.Success(_mapper.Map<CourseDto>(course), 201));
    }
}