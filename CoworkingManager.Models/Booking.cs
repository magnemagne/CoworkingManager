using System;
using System.ComponentModel.DataAnnotations;

namespace CoworkingManager.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public DateTime? LastUpdate { get; set; }

        [MaxLength(200, ErrorMessage = "Notes cannot exceed 200 characters.")]
        public string? Notes { get; set; }

        public Customer? Customer { get; set; }
        public Workstation? Workstation { get; set; }
    }
}