using Banktive.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Models.ServiceModel
{
    public class MySubscriptionsViewModel
    {
        public IEnumerable<Subscription> Subscriptions { get; set; }

        public MySubscriptionsViewModel(ApplicationDbContext db, string userName)
        {
            Subscriptions = db.Subscriptions.Include(x => x.ServiceType).Where(x => x.UserId == userName)
                .OrderBy(x => x.ServiceName);
        }
    }
}
