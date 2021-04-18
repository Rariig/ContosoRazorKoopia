using ContosoUniversityWithRazor.Data;
using ContosoUniversityWithRazor.Models;
using ContosoUniversityWithRazor.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversityWithRazor.Pages.Instructors {
    public class InstructorsModel :PageModel {

        private readonly ApplicationDbContext db;

        public InstructorsModel(ApplicationDbContext c) => db = c;

        [BindProperty]
        public Instructor Instructor { get; set; }

        public List<AssignedCourseData> AssignedCourseDataList;

        public void PopulateAssignedCourseData(ApplicationDbContext context,
                                               Instructor instructor) {
            var allCourses = context.Courses;
            var instructorCourses = new HashSet<int>(
                instructor.CourseAssignments.Select(c => c.CourseID));
            AssignedCourseDataList = new List<AssignedCourseData>();
            foreach (var course in allCourses) {
                AssignedCourseDataList.Add(new AssignedCourseData {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.CourseID)
                });
            }
        }

        public void UpdateInstructorCourses(ApplicationDbContext context,
            string[] selectedCourses, Instructor instructorToUpdate) {
            if (selectedCourses == null) {
                instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (instructorToUpdate.CourseAssignments.Select(c => c.Course.CourseID));
            foreach (var course in context.Courses) {
                if (selectedCoursesHS.Contains(course.CourseID.ToString())) {
                    if (!instructorCourses.Contains(course.CourseID)) {
                        instructorToUpdate.CourseAssignments.Add(
                            new CourseAssignment {
                                InstructorID = instructorToUpdate.ID,
                                CourseID = course.CourseID
                            });
                    }
                } else {
                    if (instructorCourses.Contains(course.CourseID)) {
                        CourseAssignment courseToRemove
                            = instructorToUpdate
                                .CourseAssignments
                                .SingleOrDefault(i => i.CourseID == course.CourseID);
                        context.Remove(courseToRemove);
                    }
                }
            }
        }

        public IActionResult OnGetCreate()
        {
            var instructor = new Instructor();
            instructor.CourseAssignments = new List<CourseAssignment>();

            // Provides an empty collection for the foreach loop
            // foreach (var course in Model.AssignedCourseDataList)
            // in the Create Razor page.
            PopulateAssignedCourseData(db, instructor);
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync(string[] selectedCourses)
        {
            var newInstructor = new Instructor();
            if (selectedCourses != null)
            {
                newInstructor.CourseAssignments = new List<CourseAssignment>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = new CourseAssignment
                    {
                        CourseID = int.Parse(course)
                    };
                    newInstructor.CourseAssignments.Add(courseToAdd);
                }
            }

            if (await TryUpdateModelAsync<Instructor>(
                newInstructor,
                "Instructor",
                i => i.FirstMidName, i => i.LastName,
                i => i.HireDate, i => i.OfficeAssignment))
            {
                db.Instructors.Add(newInstructor);
                await db.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            PopulateAssignedCourseData(db, newInstructor);
            return Page();
        }

        public async Task<IActionResult> OnGetEditAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Instructor = await db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments).ThenInclude(i => i.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Instructor == null)
            {
                return NotFound();
            }
            PopulateAssignedCourseData(db, Instructor);
            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructorToUpdate = await db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                .FirstOrDefaultAsync(s => s.ID == id);

            if (instructorToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Instructor>(
                instructorToUpdate,
                "Instructor",
                i => i.FirstMidName, i => i.LastName,
                i => i.HireDate, i => i.OfficeAssignment))
            {
                if (String.IsNullOrWhiteSpace(
                    instructorToUpdate.OfficeAssignment?.Location))
                {
                    instructorToUpdate.OfficeAssignment = null;
                }
                UpdateInstructorCourses(db, selectedCourses, instructorToUpdate);
                await db.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            UpdateInstructorCourses(db, selectedCourses, instructorToUpdate);
            PopulateAssignedCourseData(db, instructorToUpdate);
            return Page();
        }
        public async Task<IActionResult> OnGetDetailsAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Instructor = await db.Instructors.FirstOrDefaultAsync(m => m.ID == id);

            if (Instructor == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Instructor = await db.Instructors.FirstOrDefaultAsync(m => m.ID == id);

            if (Instructor == null)
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

            Instructor instructor = await db.Instructors
                .Include(i => i.CourseAssignments)
                .SingleAsync(i => i.ID == id);

            if (instructor == null)
            {
                return RedirectToPage("./Index");
            }

            var departments = await db.Departments
                .Where(d => d.InstructorID == id)
                .ToListAsync();
            departments.ForEach(d => d.InstructorID = null);

            db.Instructors.Remove(instructor);

            await db.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}