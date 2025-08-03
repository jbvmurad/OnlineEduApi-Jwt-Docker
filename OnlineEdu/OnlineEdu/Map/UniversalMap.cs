using AutoMapper;
using OnlineEdu.DTOs.DTOs.CategoryDTOs;
using OnlineEdu.DTOs.DTOs.CourseDTOs;
using OnlineEdu.Entity.Models;

namespace OnlineEdu.Map
{
    public class UniversalMap:Profile
    {
        public UniversalMap() 
        {
            ConfigureCourseMap();
            ConfigureCategoryMap();
        }

        private void ConfigureCourseMap() 
        {
            CreateMap<Course, CreateCourseDTO>().ReverseMap();
            CreateMap<Course, ResultCourseDTO>().ReverseMap();
            CreateMap<Course, UpdateCourseDTO>().ReverseMap();
        }

        private void ConfigureCategoryMap()
        {
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<Category, ResultCategoryDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
        }
    }
}
