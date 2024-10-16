using Banktive.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Models.ServiceModel
{
    public class MyRegisteredViewModel
    {
        public IEnumerable<Service> Services { get; set; }

        public MyRegisteredViewModel(ApplicationDbContext db, string? username)
        {
            Services = db.Services.Include(x => x.ServiceType).Include(x => x.Wallet).Where(x => x.UserId == username).OrderBy(x => x.Name);
        }
    }
}
