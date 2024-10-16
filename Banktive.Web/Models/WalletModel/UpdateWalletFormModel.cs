using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Models.WalletModel
{
    public class UpdateWalletFormModel
    {

        [Required]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        [StringLength(400)]
        public string? Description { get; set; }
    }
}
