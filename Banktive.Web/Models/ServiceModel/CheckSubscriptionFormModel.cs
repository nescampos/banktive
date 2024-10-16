using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Models.ServiceModel
{
    public class CheckSubscriptionFormModel
    {
        [Required]
        public Guid SubscriptionId { get; set; }

        [Required]
        public long? WalletId { get;set; }
    }
}
