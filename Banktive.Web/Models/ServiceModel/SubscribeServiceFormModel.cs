using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Models.ServiceModel
{
    public class SubscribeServiceFormModel
    {
        [Required]
        public long? ServiceId { get; set; }

        [Required]
        public string? ServiceName { get; set; }


        [Required]
        public string? CustomerId { get; set; }

        [Required]
        public int? ServiceTypeId { get; set; }
    }
}
