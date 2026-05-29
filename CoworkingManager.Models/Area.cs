using System.ComponentModel.DataAnnotations;

namespace CoworkingManager.Models
{
    public class Area
    {
        public int IdArea { get; set; }

        [Required(ErrorMessage = "Area name is required.")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Area info is required.")]
        [MaxLength(200, ErrorMessage = "Info cannot exceed 200 characters.")]
        public string Info { get; set; } = string.Empty;
    }
}