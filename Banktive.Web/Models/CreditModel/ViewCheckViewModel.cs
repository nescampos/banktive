using Banktive.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Models.CreditModel
{
    public class ViewCheckViewModel
    {
        public Check Check { get; set; }
        public Wallet WalletForCash { get; set; }
        public ViewCheckViewModel(ApplicationDbContext db, Guid id)
        {
            Check = db.Checks.Include(x => x.Currency).Include(x => x.Destination)
                .Include(x => x.CheckStatus).Include(x => x.Wallet).SingleOrDefault(x => x.Id == id);
            WalletForCash = db.Wallets.SingleOrDefault(x => x.XRPLAddress == Check.XRPLDestinationWallet);
        }
    }
}
