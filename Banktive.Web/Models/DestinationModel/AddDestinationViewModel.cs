using Banktive.Web.Data;

namespace Banktive.Web.Models.DestinationModel
{
    public class AddDestinationViewModel
    {
        public AddDestinationFormModel Form { get; set; }

        public AddDestinationViewModel(ApplicationDbContext db, string? userName)
        {
            Form = new AddDestinationFormModel();
        }
    }
}
