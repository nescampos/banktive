using Banktive.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Banktive.Web.Models.DepositModel
{
    public class CreateTimeDepositViewModel
    {
        public Wallet Wallet { get; set; }
        public IEnumerable<SelectListItem> Destinations { get; set; }
        public CreateTimeDepositFormModel Form { get; set; }

        public CreateTimeDepositViewModel(ApplicationDbContext db, long? id, string? username)
        {
            Wallet = db.Wallets.SingleOrDefault(x => x.Id == id);
            Destinations = db.Destinations.Where(x => x.UserId == username).OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            Form = new CreateTimeDepositFormModel {  OriginWalletId = id };
        }
    }
}
