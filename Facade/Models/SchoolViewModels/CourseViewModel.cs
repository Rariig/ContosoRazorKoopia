using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversityWithRazor.Models.SchoolViewModels {
    public class CourseViewModel {
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public string DepartmentName { get; set; }
    }
}
