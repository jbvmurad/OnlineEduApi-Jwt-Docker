using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineEdu.Data.Abstract;
using OnlineEdu.DTOs.DTOs.CourseDTOs;
using OnlineEdu.Entity.Models;

namespace OnlineEdu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AllAPIUsers")]
    public class CourseController(IGeneric<Course> _courseService, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetList()
        {
            var courses = _courseService.GetList();
            if (courses == null)
            {
                return NotFound("No courses found.");
            }
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var course = _courseService.GetById(id);
            if (course == null)
            {
                return NotFound("No courses found.");
            }
            return Ok(course);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "SuperAdminOnly")]
        public IActionResult Delete(int id)
        {
            var course = _courseService.GetById(id);
            if (course == null)
            {
                return NotFound("Course not found.");
            }
            _courseService.Delete(id);
            return Ok("Course deleted successfully.");
        }

        [HttpPost]
        [Authorize(Policy = "WriterOrAbove")]
        public IActionResult Create(CreateCourseDTO createCourseDTO)
        {
            var newcourse = _mapper.Map<Course>(createCourseDTO);
            _courseService.Add(newcourse);
            return Ok("Course created successfully");
        }

        [HttpPut]
        [Authorize(Policy = "SuperAdminOnly")]
        public IActionResult Update(UpdateCourseDTO updateCourseDTO)
        {
            var course = _mapper.Map<Course>(updateCourseDTO);
            _courseService.Update(course);
            return Ok("Course updated successfully.");
        }
    }
}
