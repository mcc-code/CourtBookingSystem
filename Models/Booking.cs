using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourtBookingApp.Models
{
    public class Booking
    {

        public int Id { get; set; }
        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Booking Date")]
        public DateTime? BookingDate { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public int CourtId { get; set; }
        public Court Court { get; set; }

        public int BookingStatusId { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public List<BookedSlot> BookedSlots { get; set; }
    }
}
