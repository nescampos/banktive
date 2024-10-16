using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Models.PaymentModel
{
    public class SendRegularPaymentFormModel
    {
        [Required]
        public long? OriginWalletId { get; set; }

        [Required]
        public decimal? Amount { get; set; }

        [Required]
        public Guid? DestinationId { get; set; }

        [MaxLength(200), MinLength(1)]
        public string? Comments { get; set; }
    }
}
