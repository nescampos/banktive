using Banktive.Web.Data;

namespace Banktive.Web.Models.DestinationModel
{
    public class IndexDestinationViewModel
    {
        public IndexDestinationFormModel Form { get; set; }
        public IEnumerable<Destination> Destinations { get; set; }

        public IndexDestinationViewModel(ApplicationDbContext db, string? username)
        {
            Destinations = db.Destinations.Where(x => x.UserId == username).OrderBy(x => x.Name);
        }
    }
}
