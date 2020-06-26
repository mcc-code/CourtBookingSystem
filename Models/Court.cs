using System.ComponentModel.DataAnnotations;

namespace CourtBookingApp.Models
{
    public class Court
    {
        public int Id { get; set; }

        [Display(Name = "Court Name")]
        public string CourtName { get; set; }
    }
}
