using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banktive.Web.Data
{
    public class Profile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(100), MinLength(1)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100), MinLength(1)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(200), MinLength(1)]
        public string AddressLine1 { get; set; }

        //[Required]
        [MaxLength(200), MinLength(1)]
        public string? AddressLine2 { get; set; }

        [Required]
        [MaxLength(100), MinLength(1)]
        public string City { get; set; }

        [Required]
        [MaxLength(30), MinLength(1)]
        public string TaxId { get; set; }

        [Required]
        public int? CountryId { get; set; }

        [Required]
        [MaxLength(15), MinLength(1)]
        public string PostalCode { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        [ForeignKey("CountryId")]
        public Country Country { get; set; }
    }
}
