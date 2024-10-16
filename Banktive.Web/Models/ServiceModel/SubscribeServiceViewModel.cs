using Banktive.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Banktive.Web.Models.ServiceModel
{
    public class SubscribeServiceViewModel
    {
        public SubscribeServiceFormModel Form { get; set; }
        public IEnumerable<SelectListItem> ServiceTypes { get; set; }

        public SubscribeServiceViewModel(ApplicationDbContext db, string? name)
        {
            ServiceTypes = db.ServiceTypes
                .OrderBy(x => x.Name).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
        }
    }
}
