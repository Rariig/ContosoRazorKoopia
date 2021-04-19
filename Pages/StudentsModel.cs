using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversityWithRazor.Data;
using ContosoUniversityWithRazor.Models;
using ContosoUniversityWithRazor.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Contoso.Soft.Pages.Students
{
    public class StudentsModel :PageModel
    {
        private readonly ApplicationDbContext db;

        public StudentsModel(ApplicationDbContext c) => db = c;

        [BindProperty]
        public Student Student { get; set; }
        public string ErrorMessage { get; set; }

        [BindProperty]
        public StudentVM StudentVM { get; set; }

        public async Task<IActionResult> OnGetDeleteAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await db.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Student == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await db.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            try
            {
                db.Students.Remove(student);
                await db.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./Delete",
                    new {id, saveChangesError = true});
            }
        }
        public IActionResult OnGetCreate()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var entry = db.Add(new Student());
            entry.CurrentValues.SetValues(StudentVM);
            await db.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnGetDetailsAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await db.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (Student == null)
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

            Student = await db.Students.FindAsync(id);

            if (Student == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            var studentToUpdate = await db.Students.FindAsync(id);

            if (studentToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Student>(
                studentToUpdate,
                "student",
                s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
            {
                await db.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }

    }
}
