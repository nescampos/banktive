using System.ComponentModel.DataAnnotations;

namespace Banktive.Web.Models.DepositModel
{
    public class ConfirmTimeDepositFormModel
    {
        [Required]
        public Guid Id { get; set; }

        //[Required]
        //public string? XRPLSequence { get; set; }
        public string? Cancelled { get; set; }
    }
}
