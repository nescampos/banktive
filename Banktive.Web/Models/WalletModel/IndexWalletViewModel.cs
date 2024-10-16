using Banktive.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Models.WalletModel
{
    public class IndexWalletViewModel
    {
        public IEnumerable<Wallet> Wallets { get; set; }
        public Profile Profile { get; set; }

        public IndexWalletViewModel(ApplicationDbContext db, string userName)
        {
            Profile = db.Profiles.SingleOrDefault(x => x.UserId == userName);
            Wallets = db.Wallets.Include(x => x.Currency).Where(x => x.UserId == userName)
                .OrderBy(x => x.Name);
        }
    }
}
