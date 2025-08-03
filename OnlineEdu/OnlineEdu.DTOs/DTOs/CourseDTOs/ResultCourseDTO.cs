using OnlineEdu.DTOs.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEdu.DTOs.DTOs.CourseDTOs
{
    public class ResultCourseDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int CategoryId { get; set; }
        public ResultCategoryDTO Category { get; set; }
    }
}
