using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banktive.Web.Data
{
    public class CreditWallet
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public decimal UsedAmount { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        [ForeignKey("Id")]
        public Wallet Wallet { get; set; }
    }
}
