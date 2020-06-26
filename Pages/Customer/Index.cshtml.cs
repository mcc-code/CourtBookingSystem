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
    public class IndexModel : PageModel
    {
        private readonly CourtBookingApp.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(CourtBookingApp.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ApplicationUser ApplicationUser { get; set; }
        public IList<Booking> Booking { get;set; }
        public BookingStatus BookingStatus { get; set; }
        public string StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            DateTime TodayDate = DateTime.Today;
            Booking = await _context.Booking
                .Include(b => b.ApplicationUser)
                .Include(b => b.Court)
                .Include(b => b.BookingStatus)
                .Include(b => b.BookedSlots)
                .Where(a => a.ApplicationUserId == ApplicationUser.Id && a.BookingDate >=TodayDate )
                .OrderByDescending(a => a.BookingDate)
                .ToListAsync();
        }
    }
}
