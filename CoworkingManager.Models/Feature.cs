using System.ComponentModel.DataAnnotations;

namespace CoworkingManager.Models
{
    public class Feature
    {
        public int IdFeatures { get; set; }

        [Required(ErrorMessage = "Feature name is required.")]
        [MaxLength(45, ErrorMessage = "Name cannot exceed 45 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string Description { get; set; } = string.Empty;
    }
}