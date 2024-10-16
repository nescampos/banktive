using Banktive.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Models.DepositModel
{
    public class ConfirmTimeDepositViewModel
    {
        public Deposit Deposit { get; set; }

        public ConfirmTimeDepositFormModel Form { get; set; }

        public ConfirmTimeDepositViewModel(ApplicationDbContext db, Guid id)
        {
            Deposit = db.Deposits.Include(x => x.Wallet).Include(x => x.Currency).Include(x => x.Destination).SingleOrDefault(x => x.Id == id);
            Form = new ConfirmTimeDepositFormModel { Id = id };
        }

    }
}
