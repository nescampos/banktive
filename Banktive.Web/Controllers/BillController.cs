using Banktive.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace Banktive.Web.Controllers
{
    public class BillController : Controller
    {
        private ApplicationDbContext _db;

        public BillController(ApplicationDbContext db)
        {
            _db = db;
        }
    }
}
