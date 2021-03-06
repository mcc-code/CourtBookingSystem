﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourtBookingApp.Data;
using CourtBookingApp.Models;

namespace CourtBookingApp.Pages.Admin.CheckBooking
{
    public class DetailsModel : PageModel
    {
        private readonly CourtBookingApp.Data.ApplicationDbContext _context;

        public DetailsModel(CourtBookingApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Booking Booking { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Booking = await _context.Booking
                .Include(b => b.ApplicationUser)
                .Include(b => b.BookingStatus)
                .Include(b=> b.BookedSlots)
                .Include(b => b.Court)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Booking == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
