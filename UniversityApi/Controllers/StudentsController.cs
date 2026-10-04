using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniversityApi.Common;
using UniversityApi.DTO;
using UniversityApi.Models;
using UniversityApi.Repositories;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IRepository<Student> _studentRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IRepository<Student> studentRepo, IMapper mapper, ILogger<StudentsController> logger)
    {
        _studentRepo = studentRepo;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ReturnResult<IEnumerable<StudentDto>>>> GetAll()
    {
        _logger.LogInformation("Getting all students");
        var students = await _studentRepo.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<StudentDto>>(students);
        return Ok(ReturnResult<IEnumerable<StudentDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReturnResult<StudentDto>>> GetById(int id)
    {
        var student = await _studentRepo.GetByIdAsync(id);
        if (student == null)
        {
            _logger.LogWarning("Student with id {Id} not found", id);
            return NotFound(ReturnResult<StudentDto>.Fail("STUDENT_NOT_FOUND", "Student not found", 404));
        }

        return Ok(ReturnResult<StudentDto>.Success(_mapper.Map<StudentDto>(student)));
    }

    [HttpPost]
    public async Task<ActionResult<ReturnResult<StudentDto>>> Create(StudentCreateDto dto)
    {
        var student = _mapper.Map<Student>(dto);
        await _studentRepo.AddAsync(student);
        _logger.LogInformation("Student created with id {Id}", student.Id);
        return StatusCode(201, ReturnResult<StudentDto>.Success(_mapper.Map<StudentDto>(student), 201));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ReturnResult<StudentDto>>> Update(int id, StudentUpdateDto dto)
    {
        var student = await _studentRepo.GetByIdAsync(id);
        if (student == null)
        {
            return NotFound(ReturnResult<StudentDto>.Fail("STUDENT_NOT_FOUND", "Student not found", 404));
        }

        _mapper.Map(dto, student);
        await _studentRepo.UpdateAsync(student);
        _logger.LogInformation("Student updated with id {Id}", id);
        return Ok(ReturnResult<StudentDto>.Success(_mapper.Map<StudentDto>(student)));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ReturnResult<string>>> Delete(int id)
    {
        var student = await _studentRepo.GetByIdAsync(id);
        if (student == null)
        {
            return NotFound(ReturnResult<string>.Fail("STUDENT_NOT_FOUND", "Student not found", 404));
        }

        await _studentRepo.DeleteAsync(student);
        _logger.LogInformation("Student deleted with id {Id}", id);
        return Ok(ReturnResult<string>.Success("Student deleted successfully"));
    }
}