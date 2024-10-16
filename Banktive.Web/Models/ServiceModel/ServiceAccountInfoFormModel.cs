using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Models.ServiceModel
{
    public class ServiceAccountInfoFormModel
    {
        [Required]
        public decimal? Amount { get; set; }

        [Required]
        public long ServiceAccountId { get; set; }

        [Required]
        public DateTime? DueDate { get; set; }
    }
}
