using Banktive.Web.Data;

namespace Banktive.Web.Models.WalletModel
{
    public class UpdateWalletViewModel
    {
        public Wallet Wallet { get; set; }
        public UpdateWalletFormModel Form { get; set; }

        public UpdateWalletViewModel(ApplicationDbContext db, long id)
        {
            Wallet = db.Wallets.SingleOrDefault(x => x.Id == id);
            if(Wallet != null)
            {
                Form = new UpdateWalletFormModel { Description = Wallet.Description, Id = Wallet.Id, Name = Wallet.Name };
            }
            
        }
    }
}
