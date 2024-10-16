using Banktive.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Models.DepositModel
{
    public class SelectWalletViewModel
    {
        public IEnumerable<Wallet> Wallets { get; set; }

        public SelectWalletViewModel(ApplicationDbContext db, string? name)
        {
            Wallets = db.Wallets.Include(x => x.Currency).Where(x => x.UserId == name).OrderBy(x => x.Name);
        }
    }
}
