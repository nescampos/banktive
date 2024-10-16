using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Models.DestinationModel
{
    public class AddDestinationFormModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Account { get; set; }



    }
}
