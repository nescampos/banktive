using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Models.ServiceModel
{
    public class ServiceAccountsFormModel
    {
        [Required]
        [MaxLength(100), MinLength(1)]
        public string? CustomerId { get; set; }


        [Required]
        public long Id { get; set; }
    }
}
