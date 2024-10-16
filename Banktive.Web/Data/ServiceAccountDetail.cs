using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banktive.Web.Data
{
    public class ServiceAccountDetail
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [Required]
        public DateTime DueDate {get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public bool Paid { get; set; }


        [Required]
        public long ServiceAccountId { get; set; }

        [Required]
        [ForeignKey("ServiceAccountId")]
        public ServiceAccount ServiceAccount { get; set; }

        [Required]
        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }
    }
}
