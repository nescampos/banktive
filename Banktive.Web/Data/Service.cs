using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banktive.Web.Data
{
    public class Service
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(100), MinLength(1)]
        public string? Name { get; set; }


        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public long WalletId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ServiceTypeId { get; set; }

        [Required]
        [ForeignKey("WalletId")]
        public Wallet Wallet { get; set; }

        [Required]
        [ForeignKey("ServiceTypeId")]
        public ServiceType ServiceType { get; set; }
    }
}
