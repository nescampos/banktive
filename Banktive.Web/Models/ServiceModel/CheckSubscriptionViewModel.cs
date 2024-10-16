using Banktive.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Models.ServiceModel
{
    public class CheckSubscriptionViewModel
    {
        public Subscription Subscription { get;set; }
        public CheckSubscriptionFormModel Form { get; set; }
        public IEnumerable<SelectListItem> Wallets { get;set;}
        public ServiceAccountDetail PendingAccount { get;set; }


        public CheckSubscriptionViewModel(ApplicationDbContext db, Guid id, string userName)
        {
            Subscription = db.Subscriptions.SingleOrDefault(s => s.Id == id);
            Form = new CheckSubscriptionFormModel { SubscriptionId = id };
            Wallets = db.Wallets.Where(x => x.UserId == userName)
                .OrderBy(x => x.Name).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            PendingAccount = db.ServiceAccountDetails.Include(x => x.Currency).Include(x => x.ServiceAccount)
                .ThenInclude(x => x.Service).ThenInclude(x => x.Wallet).Where(x => x.ServiceAccount.ServiceId == Subscription.ServiceId && x.ServiceAccount.CustomerId == Subscription.CustomerId 
                && x.ServiceAccount.Service.ServiceTypeId == Subscription.ServiceTypeId && x.DueDate >= DateTime.UtcNow && x.Paid == false)
                .OrderBy(x=> x.DueDate).FirstOrDefault();
        }

        

    }
}
