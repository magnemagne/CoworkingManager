using System.ComponentModel.DataAnnotations;

namespace CoworkingManager.Models
{
    public class Status
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Status value is required.")]
        [MaxLength(200, ErrorMessage = "Status cannot exceed 200 characters.")]
        public string StatusValue { get; set; } = string.Empty;

        public Booking? Booking { get; set; }
    }
}