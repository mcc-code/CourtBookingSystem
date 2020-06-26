using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourtBookingApp.Data;
using CourtBookingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CourtBookingApp.Pages.Customer
{
    [Authorize(Roles = "customer")]
    public class DetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CourtBookingApp.Data.ApplicationDbContext _context;

        public DetailsModel(UserManager<ApplicationUser> userManager, CourtBookingApp.Data.ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public ApplicationUser ApplicationUser { get; set; }
        public Booking Booking { get; set; }
        public IList<BookedSlot> BookedSlot { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser = await _userManager.GetUserAsync(User);  // User = logged in user (built in magic)
            if (ApplicationUser == null)
            {
                return NotFound();
            }

            Booking = await _context.Booking
                    .Include(b => b.ApplicationUser)
                    .Include(b => b.Court)
                     .Include(b => b.BookingStatus)
                    .Include(b => b.BookedSlots)
                    .FirstOrDefaultAsync(m => m.Id == id);

            BookedSlot = await _context.BookedSlot
                .Include(b => b.Booking)
                .Where(b => b.BookingId == id)
                .ToListAsync();

            if (Booking == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
