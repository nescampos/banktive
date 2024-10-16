using Banktive.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Models.PaymentModel
{
    public class ApproveRegularPaymentViewModel
    {
        public Payment Payment { get; set; }
        public ApproveRegularPaymentFormModel Form { get; set; }

        public ApproveRegularPaymentViewModel(ApplicationDbContext db, Guid id)
        {
            Form = new ApproveRegularPaymentFormModel {  PaymentId = id };
            Payment = db.Payments.Include(x => x.Wallet).Include(x => x.Currency).Include(x => x.Destination)
                .Include(x => x.PaymentStatus).Include(x => x.PaymentType).SingleOrDefault(x => x.Id == id);
        }
    }
}
