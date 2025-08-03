using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEdu.Entity.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public List<Course> Courses { get; set; } 
    }
}
