using Banktive.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Models.ServiceModel
{
    public class MyServiceViewModel
    {
        public Service Service { get; set; }

        public MyServiceViewModel(ApplicationDbContext db, long id)
        {
            Service = db.Services.Include(x => x.ServiceType).Include(x => x.Wallet).SingleOrDefault(x => x.Id == id);
        }
    }
}
