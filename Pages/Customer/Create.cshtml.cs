using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CourtBookingApp.Data;
using CourtBookingApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace CourtBookingApp.Pages.Customer
{
    [Authorize(Roles = "customer")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CourtBookingApp.Data.ApplicationDbContext _context;

        public CreateModel(UserManager<ApplicationUser> userManager, CourtBookingApp.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        //Properties for Get
        public ApplicationUser ApplicationUser { get; set; }

        //Select Date
        [BindProperty(SupportsGet = true)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime? SelectedBookingDate { get; set; }
        public string SelectedDate { get; set; }
        public string SelectedDOW { get; set; } 
        public int AvailableDate { get; set; }

        //Select Court
        public IList<Court> Courts { get; set; }
        public IList<SelectListItem> CourtsList { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SelectedCourtId { get; set; }
        public string SelectedCourtName { get; set; }

        //Select Slot
        public IList<SelectListItem> Slots { get; set; }
        public IList<BookedSlot> UnavailableSlots { get; set; }


        //Properties for Post
        [BindProperty]
        public List<int> AreChecked { get; set; }
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            //Check valid user//////////////////////////////////////////////////////////////////////////////////
            ApplicationUser = await _userManager.GetUserAsync(User);  // User = logged in user (built in magic)
            if (ApplicationUser == null)
            {
                return NotFound();
            }
            bool isCustomer = await _userManager.IsInRoleAsync(ApplicationUser, "customer");
            if (!isCustomer)
            {
                return NotFound();
            }

            //Deal with Courts Select List///////////////////////////////////////////////////////////////////////////
            Courts = await _context.Court.OrderBy(c => c.Id).ToListAsync();
            CourtsList = new List<SelectListItem>();

            foreach (var item in Courts)
            {
                CourtsList.Add(new SelectListItem(item.CourtName, item.Id.ToString()));
            }

            // Deal with OnGET Court///////////////////////////////////////////////////////////////////////////
            if (string.IsNullOrEmpty(SelectedCourtId))
            {
                SelectedCourtId = Courts[0].Id.ToString();
                SelectedCourtName = Courts[0].CourtName;
            }
            else
            {
                int courtId = int.Parse(SelectedCourtId);
                foreach (var c in Courts)
                {
                    if (c.Id == courtId)
                        SelectedCourtName = c.CourtName;
                }
            }

            // Deal with Date Select Input ///////////////////////////////////////////////////////////////////////////
            DateTime date1 = SelectedBookingDate ?? DateTime.Today;             // this is 2020 06 20
            DateTime MaxDate = DateTime.Today;                       // this is also 2020 06 20
            AvailableDate = DateTime.Compare(date1, MaxDate);         // this return 0
            SelectedBookingDate = date1;
            SelectedDate = date1.ToString("dddd, dd/MM/yyyy");
            SelectedDOW = date1.ToString("dddd");


            //Deal with Time Slots List/////////////////////////////////////////////////////////////////////////////////////
            DateTime openTime = new DateTime(2000, 01, 01, 08, 00, 00);
            DateTime[] openTimeArr = new DateTime[35];
            for (int i = 0; i < 35; i++)
            {
                openTimeArr[i] = openTime;
                openTime = openTime.AddMinutes(30.00);
            }

            Slots = new List<SelectListItem>();
            for (int i = 0; i < 34; i++)
            {
                Slots.Add(new SelectListItem(openTimeArr[i].ToString("hh:mm tt")+ " - " + openTimeArr[i+1].ToString("hh:mm tt"), (i + 1).ToString()));
            }

            //Find the booked slots//////////////////////////////////////////////////////////////////////////////// 
            int cid = int.Parse(SelectedCourtId);
            int DeniedId = await _context.BookingStatus.Where(a => a.Status == "Denied").Select(a=> a.Id).SingleOrDefaultAsync();
         

            UnavailableSlots = await _context.BookedSlot.Include(a => a.Booking)
                .Where(b => b.Booking.BookingDate == SelectedBookingDate && 
                            b.Booking.CourtId == cid && 
                            b.Booking.BookingStatusId != DeniedId)
                .ToListAsync();

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            //Check valid user//////////////////////////////////////////////////////////////////////////////////
            ApplicationUser = await _userManager.GetUserAsync(User);  // User = logged in user (built in magic)

            if (ApplicationUser == null)
            {
                return NotFound();
            }
            bool isCustomer = await _userManager.IsInRoleAsync(ApplicationUser, "customer");
            if (!isCustomer)
            {
                return NotFound();
            }

            int SelectedCourt;
            if (SelectedCourtId == null )
            {
                SelectedCourt = 1;
            }
            else
            {
                SelectedCourt = int.Parse(SelectedCourtId);
            }
           
            int BookingStatusId = await _context.BookingStatus
                .Where(a => a.Status == "Pending")
                .Select(a => a.Id).SingleOrDefaultAsync();

            var booking = new Booking()
            {
                ApplicationUserId = ApplicationUser.Id,
                CreatedDate = DateTime.Now,
                BookingDate = SelectedBookingDate,
                CourtId = SelectedCourt,
                BookingStatusId = BookingStatusId
            };
            _context.Booking.Add(booking);
            await _context.SaveChangesAsync();

            for (var i = 0; i < AreChecked.Count(); i++)
            {
                var slot = AreChecked[i] + 1;
                var bookedSlots = new BookedSlot()
                {
                    Slot= slot,
                    BookingId = booking.Id
                };
                _context.BookedSlot.Add(bookedSlots);
                await _context.SaveChangesAsync();
            }

            StatusMessage = "New booking has been added";
            return RedirectToPage("./Index");
        }
    }
}
