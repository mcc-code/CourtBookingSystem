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

namespace CourtBookingApp.Pages.Admin.CheckBooking
{
    [Authorize(Roles =  "admin")]
    public class BookingHistoryModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CourtBookingApp.Data.ApplicationDbContext _context;

        public BookingHistoryModel(UserManager<ApplicationUser> userManager, CourtBookingApp.Data.ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // For OnGET
        public ApplicationUser ApplicationUser { get; set; }
        public IList<Booking> BookingList { get;  set; }


        // For OnPOST
        [BindProperty]
        public string SelectedStatus { get; set; }
        [BindProperty]
        public int SelectedId {get;set;}
        [BindProperty]
        public Booking Booking { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            DateTime TodayDate = DateTime.Today;
            BookingList = await _context.Booking
                .Include(b => b.ApplicationUser)
                .Include(b => b.BookingStatus)
                .Include(b => b.Court)
                .Include(b => b.BookedSlots)
                .Where(b => b.BookingDate < TodayDate)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
            return Page();
        }


        public async Task<ActionResult> OnPostAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);  // User = logged in user (built in magic)

            if (ApplicationUser == null)
            {
                return NotFound();
            }
            bool isAdmin = await _userManager.IsInRoleAsync(ApplicationUser, "admin");
            if (!isAdmin)
            {
                return NotFound();
            }

            int SelectedStatusId = await _context.BookingStatus.Where(a => a.Status == SelectedStatus).Select(a => a.Id).SingleOrDefaultAsync() ;// find status id here
            Booking = await _context.Booking.Where(a => a.Id == SelectedId).SingleOrDefaultAsync();
            if (Booking == null)
            {
                return NotFound();
            }
            Booking.BookingStatusId = SelectedStatusId;

            //_context.Attach(Booking).State = EntityState.Modified;
            _context.Update(Booking);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
