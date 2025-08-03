using OnlineEdu.DTOs.DTOs.CourseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEdu.DTOs.DTOs.CategoryDTOs
{
    public class ResultCategoryDTO
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public List<ResultCourseDTO> Courses { get; set; }
    }
}
