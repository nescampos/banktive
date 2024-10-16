using Banktive.Web.Data;

namespace Banktive.Web.Models.DestinationModel
{
    public class EditDestinationViewModel
    {
        public Destination Destination { get; set; }
        public EditDestinationFormModel Form { get; set; }
        public bool ExistPayments { get; set; }

        public EditDestinationViewModel(ApplicationDbContext db, Guid id)
        {
            Destination = db.Destinations.SingleOrDefault(x => x.Id == id);
            if(Destination != null)
            {
                Form = new EditDestinationFormModel
                {
                    //Account = Destination.Account,
                    Email = Destination.Email,
                    Id = id,
                    Name = Destination.Name
                };
                
            }
            ExistPayments = db.Payments.Any(x => x.DestinationId == id);
        }
    }
}
