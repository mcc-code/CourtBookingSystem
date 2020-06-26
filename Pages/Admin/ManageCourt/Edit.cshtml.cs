using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourtBookingApp.Data;
using CourtBookingApp.Models;

namespace CourtBookingApp.Pages.Admin.ManageCourt
{
    public class EditModel : PageModel
    {
        private readonly CourtBookingApp.Data.ApplicationDbContext _context;

        public EditModel(CourtBookingApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Court Court { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Court = await _context.Court.FirstOrDefaultAsync(m => m.Id == id);

            if (Court == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Court).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourtExists(Court.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CourtExists(int id)
        {
            return _context.Court.Any(e => e.Id == id);
        }
    }
}
