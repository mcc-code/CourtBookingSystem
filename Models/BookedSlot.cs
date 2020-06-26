using System.ComponentModel.DataAnnotations;

namespace CourtBookingApp.Models
{
    public class BookedSlot
    {
        public int Id { get; set; }

        [Required]  // required for cascade on delete Booking
        public int BookingId { get; set; }
        public Booking Booking { get; set; }


        [Display(Name = "Booked Time Slot")]
        public int Slot { get; set; }
    }
}
