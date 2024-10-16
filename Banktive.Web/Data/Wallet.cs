using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banktive.Web.Data
{
    public class Wallet
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(100), MinLength(1)]
        public string? Name { get; set; }

        //[Required]
        [MaxLength(400), MinLength(1)]
        public string? Description { get; set; }

        [Required]
        public int? CurrencyId { get; set; }

        [Required]
        [MaxLength(100), MinLength(5)]
        
        public string? Alias { get; set; }

        [Required]
        public string? XRPLAddress { get; set; }

        [Required]
        public string? XRPLSeed { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }
    }
}
