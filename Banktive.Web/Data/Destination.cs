using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Data
{
    public class Destination
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(400)]
        public string? Name { get; set; }

        [Required]
        [StringLength(400)]
        public string? Email { get; set; }

        [Required]
        [StringLength(200)]
        public string? Account { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime LastUpdatedAt { get; set; }

        [Required]
        public string? UserId { get; set; }
    }
}
