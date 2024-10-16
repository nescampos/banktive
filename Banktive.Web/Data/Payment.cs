using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banktive.Web.Data
{
    public class Payment
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public long OriginWalletId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [Required]
        public int PaymentStatusId { get; set; }

        [Required]
        public int PaymentTypeId { get; set; }

        [Required]
        [MaxLength(200), MinLength(1)]
        public string? XRPLDestinationWallet { get; set; }

        public Guid? DestinationId { get; set; }

        [MaxLength(200), MinLength(1)]
        public string? Comments { get; set; }

        public decimal? Fee { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? ConfirmationAt { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }

        [Required]
        [ForeignKey("PaymentTypeId")]
        public PaymentType PaymentType { get; set; }

        [Required]
        [ForeignKey("PaymentStatusId")]
        public PaymentStatus PaymentStatus { get; set; }


        [Required]
        [ForeignKey("OriginWalletId")]
        public Wallet Wallet { get; set; }

        [ForeignKey("DestinationId")]
        public Destination? Destination { get; set; }
    }
}
