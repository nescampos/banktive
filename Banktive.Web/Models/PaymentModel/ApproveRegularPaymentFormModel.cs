using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Models.PaymentModel
{
    public class ApproveRegularPaymentFormModel
    {
        [Required]
        public Guid PaymentId { get; set; }

        //[Required]
        //public string? Password { get; set; }

        public string? Cancelled { get; set; }
    }
}
