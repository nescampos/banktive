using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Models.SettingModel
{
    public class UpdateProfileFormModel
    {
        [Required]
        [StringLength(100)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string? LastName { get; set; }

        [Required]
        [StringLength(200)]
        public string? AddressLine1 { get; set; }

        //[Required]
        [StringLength(200)]
        public string? AddressLine2 { get; set; }

        [Required]
        [StringLength(100)]
        public string? City { get; set; }

        [Required]
        [StringLength(30)]
        public string? TaxId { get; set; }

        [Required]
        public int? CountryId { get; set; }

        [Required]
        [StringLength(15)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "The Postal code must only contain numbers.")]
        public string? PostalCode { get; set; }
    }
}
