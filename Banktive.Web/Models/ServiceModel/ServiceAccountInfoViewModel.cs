using Banktive.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Models.ServiceModel
{
    public class ServiceAccountInfoViewModel
    {
        public ServiceAccount ServiceAccount { get; set; }
        public IEnumerable<ServiceAccountDetail> ServiceAccountDetails { get; set; }
        public ServiceAccountInfoFormModel Form { get; set; }

        public ServiceAccountInfoViewModel(ApplicationDbContext db, long id)
        {
            Form = new ServiceAccountInfoFormModel { ServiceAccountId = id };
            ServiceAccount = db.ServiceAccounts.SingleOrDefault(x => x.Id == id);
            ServiceAccountDetails = db.ServiceAccountDetails.Include(x => x.Currency).Where(x => x.ServiceAccountId == id).OrderByDescending(x => x.DueDate);
        }
    }
}
