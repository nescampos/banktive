using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Models.CreditModel
{
    public class PayWithCheckFormModel
    {
        [Required]
        public long OriginWalletId { get; set; }

        [Required]
        public decimal? Amount { get; set; }


        public Guid? DestinationId { get; set; }

        [MaxLength(200), MinLength(1)]
        public string? Comments { get; set; }

        public string? CheckId { get; set; }

        [Required]
        public DateTime? DateToCash { get; set; }
    }
}
