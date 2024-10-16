using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Models.ServiceModel
{
    public class RegisterServiceFormModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public long? WalletId { get; set; }

        [Required]
        public int? ServiceTypeId { get; set; }
    }
}
