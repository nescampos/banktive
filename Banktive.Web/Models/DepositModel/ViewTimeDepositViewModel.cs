using Banktive.Web.Data;
using Microsoft.EntityFrameworkCore;
namespace Banktive.Web.Models.DepositModel
{
    public class ViewTimeDepositViewModel
    {
        public Deposit Deposit { get; set; }
        public Wallet WalletForCash { get; set; }
        public ViewTimeDepositViewModel(ApplicationDbContext db, Guid id)
        {
            Deposit = db.Deposits.Include(x => x.Currency).Include(x => x.Destination).Include(x => x.Wallet)
                .Include(x => x.DepositStatus).SingleOrDefault(x => x.Id == id);

            WalletForCash = db.Wallets.SingleOrDefault(x => x.XRPLAddress == Deposit.XRPLDestinationWallet);
        }
    }
}
