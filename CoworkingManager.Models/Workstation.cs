using System;
using System.ComponentModel.DataAnnotations;

namespace CoworkingManager.Models
{
    public class Workstation
    {
        public int Id { get; set; }

        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string? Description { get; set; }

        public TimeSpan? Opening { get; set; }

        public TimeSpan? Closing { get; set; }

        public int? MaxReservations { get; set; }

        public Area? Area { get; set; }
    }
}