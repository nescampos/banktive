using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Models.DepositModel
{
    public class CreateTimeDepositFormModel
    {
        [Required]
        public long? OriginWalletId { get; set; }

        [Required]
        public decimal? Amount { get; set; }

        [Required]
        public Guid? DestinationId { get; set; }

        [Required]
        public DateTime? DateToCash { get; set; }

        [MaxLength(200), MinLength(1)]
        public string? Comments { get; set; }
    }
}
