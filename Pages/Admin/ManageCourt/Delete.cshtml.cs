using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourtBookingApp.Data;
using CourtBookingApp.Models;

namespace CourtBookingApp.Pages.Admin.ManageCourt
{
    public class DeleteModel : PageModel
    {
        private readonly CourtBookingApp.Data.ApplicationDbContext _context;

        public DeleteModel(CourtBookingApp.Data.ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Court = await _context.Court.FindAsync(id);

            if (Court != null)
            {
                _context.Court.Remove(Court);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
