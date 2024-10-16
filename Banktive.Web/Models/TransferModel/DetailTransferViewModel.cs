using Banktive.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Models.TransferModel
{
    public class DetailTransferViewModel
    {
        public Payment Payment { get; set; }

        public DetailTransferViewModel(ApplicationDbContext db, Guid id)
        {
            Payment = db.Payments.Include(x => x.Currency).Include(x => x.Destination)
                .Include(x => x.PaymentStatus).Include(x => x.PaymentType).Include(x => x.Wallet).SingleOrDefault(x => x.Id == id);
        }
    }
}
