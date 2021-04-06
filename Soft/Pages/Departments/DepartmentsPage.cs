using System.Threading.Tasks;
using ContosoUniversityWithRazor.Data;
using ContosoUniversityWithRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Contoso.Soft.Pages.Departments
{
    public class DepartmentsPage : PageModel
    {
        private readonly ApplicationDbContext db;

        public DepartmentsPage(ApplicationDbContext c) => db = c;

            public IActionResult OnGet()
        {
            ViewData["InstructorID"] = new SelectList(db.Instructors, "ID", "Discriminator");
            return Page();
        }

        [BindProperty] public Department Department { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            db.Departments.Add(Department);
            await db.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
