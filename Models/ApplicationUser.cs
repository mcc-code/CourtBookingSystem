using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourtBookingApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Name")]
        public string FullName { get; set; }

        [Display(Name = "I/C No")]
        public string ICNo { get; set; }

        public List<Booking> Booking { get; set; }
    }
}
