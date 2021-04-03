using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversityWithRazor.Data;
using ContosoUniversityWithRazor.Models;
using ContosoUniversityWithRazor.Models.SchoolViewModels;

namespace ContosoUniversityWithRazor.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversityWithRazor.Data.ApplicationDbContext _context;

        public IndexModel(ContosoUniversityWithRazor.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CourseViewModel> CourseVM { get; set; }

        public async Task OnGetAsync() {
            CourseVM = await _context.Courses
                    .Select(p => new CourseViewModel {
                        CourseID = p.CourseID,
                        Title = p.Title,
                        Credits = p.Credits,
                        DepartmentName = p.Department.Name
                    }).ToListAsync();
        }
    }
}
