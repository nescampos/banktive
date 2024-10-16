using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Data
{
    public class CheckStatus
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100), MinLength(1)]
        public string? Name { get; set; }
    }
}
