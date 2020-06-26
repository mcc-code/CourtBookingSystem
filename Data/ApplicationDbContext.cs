using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CourtBookingApp.Models;

namespace CourtBookingApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CourtBookingApp.Models.Booking> Booking { get; set; }
        public DbSet<CourtBookingApp.Models.BookingStatus> BookingStatus { get; set; }
        public DbSet<CourtBookingApp.Models.Court> Court { get; set; }
        public DbSet<CourtBookingApp.Models.BookedSlot> BookedSlot { get; set; }
    }
}
