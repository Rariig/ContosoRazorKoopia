using System.Linq;
using System.Threading.Tasks;
using ContosoUniversityWithRazor.Data;
using ContosoUniversityWithRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Contoso.Soft.Pages.Courses
{
    public class CoursesModel:PageModel
    {
        private readonly ApplicationDbContext db;

        public CoursesModel(ApplicationDbContext c) => db = c;
        
        [BindProperty]
        public Course Course { get; set; }

        public SelectList DepartmentNameSL { get; set; }

        public void PopulateDepartmentsDropDownList(ApplicationDbContext db,
            object selectedDepartment = null)
        {
            var departmentsQuery = from d in db.Departments
                orderby d.Name // Sort by name.
                select d;

            DepartmentNameSL = new SelectList(departmentsQuery.AsNoTracking(),
                "DepartmentID", "Name", selectedDepartment);
        }

        public IActionResult OnGetCreate()
        {
            PopulateDepartmentsDropDownList(db);
            return Page();
        }



        public async Task<IActionResult> OnPostCreateAsync()
        {
            var emptyCourse = new Course();

            if (await TryUpdateModelAsync<Course>(
                emptyCourse,
                "course",   // Prefix for form value.
                s => s.CourseID, s => s.DepartmentID, s => s.Title, s => s.Credits))
            {
                db.Courses.Add(emptyCourse);
                await db.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            // Select DepartmentID if TryUpdateModelAsync fails.
            PopulateDepartmentsDropDownList(db, emptyCourse.DepartmentID);
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await db.Courses
                .AsNoTracking()
                .Include(c => c.Department).FirstOrDefaultAsync(m => m.CourseID == id);

            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await db.Courses.FindAsync(id);

            if (Course != null)
            {
                db.Courses.Remove(Course);
                await db.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnGetDetailsAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await db.Courses
                .AsNoTracking()
                .Include(c => c.Department).FirstOrDefaultAsync(m => m.CourseID == id);

            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnGetEditAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await db.Courses
                .Include(c => c.Department).FirstOrDefaultAsync(m => m.CourseID == id);

            if (Course == null)
            {
                return NotFound();
            }

            // Select current DepartmentID.
            PopulateDepartmentsDropDownList(db, Course.DepartmentID);
            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseToUpdate = await db.Courses.FindAsync(id);

            if (courseToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Course>(
                courseToUpdate,
                "course",   // Prefix for form value.
                c => c.Credits, c => c.DepartmentID, c => c.Title))
            {
                await db.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            // Select DepartmentID if TryUpdateModelAsync fails.
            PopulateDepartmentsDropDownList(db, courseToUpdate.DepartmentID);
            return Page();
        }

    }
}
