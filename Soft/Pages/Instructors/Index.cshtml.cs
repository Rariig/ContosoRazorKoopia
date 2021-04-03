﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversityWithRazor.Data;
using ContosoUniversityWithRazor.Models;
using ContosoUniversityWithRazor.Models.SchoolViewModels;

namespace ContosoUniversityWithRazor.Pages.Instructors {
    public class IndexModel :PageModel {
        private readonly ContosoUniversityWithRazor.Data.ApplicationDbContext _context;

        public IndexModel(ContosoUniversityWithRazor.Data.ApplicationDbContext context) {
            _context = context;
        }

        public InstructorIndexData InstructorData { get; set; }
        public int InstructorID { get; set; }
        public int CourseID { get; set; }

        public async Task OnGetAsync(int? id, int? courseID) {
            InstructorData = new InstructorIndexData();
            InstructorData.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(i => i.Department)
                .OrderBy(i => i.LastName)
                .ToListAsync();

            if (id != null) {
                InstructorID = id.Value;
                Instructor instructor = InstructorData.Instructors
                    .Single(i => i.ID == id.Value);
                InstructorData.Courses = instructor.CourseAssignments.Select(s => s.Course);
            }

            if (courseID != null) {
                CourseID = courseID.Value;
                var selectedCourse = InstructorData.Courses
                    .Single(x => x.CourseID == courseID);
                await _context.Entry(selectedCourse).Collection(x => x.Enrollments).LoadAsync();
                foreach (Enrollment enrollment in selectedCourse.Enrollments) {
                    await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync();
                }
                InstructorData.Enrollments = selectedCourse.Enrollments;
            }
        }
    }
}
