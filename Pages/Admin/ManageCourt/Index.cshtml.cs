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
    public class IndexModel : PageModel
    {
        private readonly CourtBookingApp.Data.ApplicationDbContext _context;

        public IndexModel(CourtBookingApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Court> Court { get;set; }

        public async Task OnGetAsync()
        {
            Court = await _context.Court.ToListAsync();
        }
    }
}
