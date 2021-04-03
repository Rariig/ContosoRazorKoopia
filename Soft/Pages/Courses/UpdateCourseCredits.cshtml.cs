using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversityWithRazor.Pages.Courses
{
    public class UpdateCourseCreditsModel : PageModel
    {
        private readonly ContosoUniversityWithRazor.Data.ApplicationDbContext _context;

        public UpdateCourseCreditsModel(ContosoUniversityWithRazor.Data.ApplicationDbContext context) {
            _context = context;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(double? multiplier) {
            if (multiplier != null) {
                ViewData["RowsAffected"] =
                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE Course SET Credits = Credits * {0}",
                        parameters: multiplier);
            }
            return Page();
        }
    }
}
