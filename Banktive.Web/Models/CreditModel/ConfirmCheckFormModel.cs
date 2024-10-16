using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Models.CreditModel
{
    public class ConfirmCheckFormModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? CheckXRPLId { get; set; }
        public string? Cancelled { get; set; }
    }
}
