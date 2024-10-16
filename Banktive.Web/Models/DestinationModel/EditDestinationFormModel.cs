using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Models.DestinationModel
{
    public class EditDestinationFormModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        //[Required]
        //public string? Account { get; set; }
    }
}
