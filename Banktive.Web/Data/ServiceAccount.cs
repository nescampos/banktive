using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banktive.Web.Data
{
    public class ServiceAccount
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(100), MinLength(1)]
        public string? CustomerId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public long ServiceId { get; set; }

        [Required]
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }
    }
}
