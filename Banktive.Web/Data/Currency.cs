using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Data
{
    public class Currency
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100), MinLength(1)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(20), MinLength(1)]
        public string? Code { get; set; }

        [Required]
        public bool Enabled { get; set; }
    }
}
