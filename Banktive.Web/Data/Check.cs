using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banktive.Web.Data
{
    public class Check
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
        [MaxLength(200), MinLength(1)]
        public string? XRPLDestinationWallet { get; set; }

        public Guid? DestinationId { get; set; }

        [MaxLength(200), MinLength(1)]
        public string? Comments { get; set; }

        public decimal? Fee { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime DateToCash { get; set; }

        public DateTime? CashedAt { get; set; }

        [Required]
        public string? UserId { get; set; }

        public string? CheckXRPLId { get; set; }

        public long? LedgerSequence { get; set; }

        [Required]
        public int CheckStatusId { get; set; }

        [Required]
        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }


        [Required]
        [ForeignKey("OriginWalletId")]
        public Wallet Wallet { get; set; }

        [ForeignKey("DestinationId")]
        public Destination? Destination { get; set; }

        [Required]
        [ForeignKey("CheckStatusId")]
        public CheckStatus CheckStatus { get; set; }
    }
}
