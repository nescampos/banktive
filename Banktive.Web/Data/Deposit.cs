using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banktive.Web.Data
{
    public class Deposit
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
        public int DepositStatusId { get; set; }

        [Required]
        public int DepositTypeId { get; set; }

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

        public DateTime? DateToCash { get; set; }

        public int? XRPLSequence { get; set; }
        public string? TransactionHash { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }

        [Required]
        [ForeignKey("DepositTypeId")]
        public DepositType DepositType { get; set; }

        [Required]
        [ForeignKey("DepositStatusId")]
        public DepositStatus DepositStatus { get; set; }


        [Required]
        [ForeignKey("OriginWalletId")]
        public Wallet Wallet { get; set; }

        [ForeignKey("DestinationId")]
        public Destination? Destination { get; set; }
    }
}
