using Banktive.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Banktive.Web.Models.CreditModel
{
    public class PayWithCheckViewModel
    {
        public PayWithCheckFormModel Form { get; set; }
        public Wallet Wallet { get; set; }
        public IEnumerable<SelectListItem> Destinations { get; set; }

        public PayWithCheckViewModel(ApplicationDbContext db, long? id, string? name)
        {
            Wallet = db.Wallets.SingleOrDefault(x => x.Id == id);
            Destinations = db.Destinations.Where(x => x.UserId == name).OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            Form = new PayWithCheckFormModel { OriginWalletId = id.Value };
        }

        
    }
}
