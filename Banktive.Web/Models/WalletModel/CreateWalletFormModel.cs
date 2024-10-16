using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Models.WalletModel
{
    public class CreateWalletFormModel
    {
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        [StringLength(400)]
        public string? Description { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression("^[a-z0-9]*$", ErrorMessage = "The Alias must only contain letters (lowercase) and numbers.")]
        public string? Alias { get; set; }

        [Required]
        public int? CurrencyId { get; set; }

        [Required]
        public string? XRPLAddress { get; set; }

        [Required]
        public string? XRPLSeed { get; set; }
    }
}
