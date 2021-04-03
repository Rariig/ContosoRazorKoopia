using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversityWithRazor.Data;
using ContosoUniversityWithRazor.Models;

namespace ContosoUniversityWithRazor.Pages.Departments
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversityWithRazor.Data.ApplicationDbContext _context;

        public IndexModel(ContosoUniversityWithRazor.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Department> Department { get;set; }

        public async Task OnGetAsync()
        {
            Department = await _context.Departments
                .Include(d => d.Administrator).ToListAsync();
        }
    }
}
