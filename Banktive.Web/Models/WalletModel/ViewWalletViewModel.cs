using Banktive.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Models.WalletModel
{
    public class ViewWalletViewModel
    {
        public Wallet Wallet { get; set; }

        public ViewWalletViewModel(ApplicationDbContext db, long id)
        {
            Wallet = db.Wallets.Include(x => x.Currency).SingleOrDefault(x => x.Id == id);
        }
    }
}
