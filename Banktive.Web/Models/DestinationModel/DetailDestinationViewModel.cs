using Banktive.Web.Data;

namespace Banktive.Web.Models.DestinationModel
{
    public class DetailDestinationViewModel
    {
        public Destination Destination { get; set; }

        public DetailDestinationViewModel(ApplicationDbContext db, Guid id)
        {
            Destination = db.Destinations.SingleOrDefault(x => x.Id == id);

        }
    }
}
