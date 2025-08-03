using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineEdu.Data.Abstract;
using OnlineEdu.DTOs.DTOs.CategoryDTOs;
using OnlineEdu.Entity.Models;

namespace OnlineEdu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AllAPIUsers")]
    public class CategoryController (IGeneric<Category> _categoryService , IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetList()
        {
            var categories = _categoryService.GetList();
            if (categories == null)
            {
                return NotFound("No categories found.");
            }
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound("Category not found.");
            }
            return Ok(category);
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "SuperAdminOnly")]
        public IActionResult Delete(int id)
        {
            var category = _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound("Category not found.");
            }
            _categoryService.Delete(id);
            return Ok("Category deleted successfully.");
        }
        [HttpPost]
        [Authorize(Policy = "WriterOrAbove")]
        public IActionResult Create(CreateCategoryDTO createCategoryDTO)
        {
            var newCategory = _mapper.Map<Category>(createCategoryDTO);
            _categoryService.Add(newCategory);
            return Ok("Category created successfully");
        }
        [HttpPut]
        [Authorize(Policy = "SuperAdminOnly")]
        public IActionResult Update(UpdateCategoryDTO updateCategoryDTO)
        {
            var category = _mapper.Map<Category>(updateCategoryDTO);
            _categoryService.Update(category);
            return Ok("Category updated successfully");
        }
    }
}
