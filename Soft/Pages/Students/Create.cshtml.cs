﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversityWithRazor.Data;
using ContosoUniversityWithRazor.Models;
using ContosoUniversityWithRazor.Models.SchoolViewModels;

namespace ContosoUniversityWithRazor.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly ContosoUniversityWithRazor.Data.ApplicationDbContext _context;

        public CreateModel(ContosoUniversityWithRazor.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public StudentVM StudentVM { get; set; }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            var entry = _context.Add(new Student());
            entry.CurrentValues.SetValues(StudentVM);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}