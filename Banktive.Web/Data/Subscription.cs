using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banktive.Web.Data
{
    public class Subscription
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public long ServiceId { get; set; }

        [Required]
        public string ServiceName { get; set; }


        [Required]
        public string CustomerId { get; set; }


        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ServiceTypeId { get; set; }

        [Required]
        [ForeignKey("ServiceTypeId")]
        public ServiceType ServiceType { get; set; }
    }
}
