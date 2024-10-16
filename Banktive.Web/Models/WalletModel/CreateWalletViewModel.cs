using Banktive.Web.Data;

namespace Banktive.Web.Models.WalletModel
{
    public class CreateWalletViewModel
    {
        private ApplicationDbContext db;
        public CreateWalletFormModel Form { get; set; }
        public CreateWalletViewModel(ApplicationDbContext db)
        {
            this.db = db;
        }
    }
}
