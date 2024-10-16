using Banktive.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Banktive.Web.Models.PaymentModel
{
    public class SendRegularPaymentViewModel
    {
        public Wallet Wallet { get; set; }
        public IEnumerable<SelectListItem> Destinations { get; set; }
        public SendRegularPaymentFormModel Form { get; set; }

        public SendRegularPaymentViewModel(ApplicationDbContext db, long? id, string? username)
        {
            Wallet = db.Wallets.SingleOrDefault(x => x.Id == id);
            Destinations = db.Destinations.Where(x => x.UserId == username).OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            Form = new SendRegularPaymentFormModel { OriginWalletId = id };
        }
    }
}
