using Banktive.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Models.CreditModel
{
    public class ConfirmCheckViewModel
    {
        public Check Check { get; set; }
        public ConfirmCheckFormModel Form { get; set; }

        public ConfirmCheckViewModel(ApplicationDbContext db, Guid id)
        {
            Check = db.Checks.Include(x => x.Wallet).Include(x => x.Currency).Include(x => x.Destination)
                .Include(x => x.CheckStatus).SingleOrDefault(x => x.Id == id);
            Form = new ConfirmCheckFormModel {  Id = id };
        }
    }
}
