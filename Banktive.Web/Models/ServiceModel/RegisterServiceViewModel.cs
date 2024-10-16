using Banktive.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Banktive.Web.Models.ServiceModel
{
    public class RegisterServiceViewModel
    {
        public RegisterServiceFormModel Form { get; set; }
        public IEnumerable<SelectListItem> Wallets { get; set; }
        public IEnumerable<SelectListItem> ServiceTypes { get; set; }

        public RegisterServiceViewModel(ApplicationDbContext db, string? userName)
        {
            Wallets = db.Wallets.Where(x => x.UserId == userName)
                .OrderBy(x => x.Name).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ServiceTypes = db.ServiceTypes
                .OrderBy(x => x.Name).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
        }

        
    }
}
