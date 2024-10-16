using Banktive.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Banktive.Web.Models.CreditModel
{
    public class IndexCreditViewModel
    {
        public int Page { get; set; }
        public IEnumerable<CreditDTO> Checks { get; set; }
        public IEnumerable<SelectListItem> Months { get; set; }
        public IEnumerable<SelectListItem> Years { get; set; }
        public IEnumerable<SelectListItem> Wallets { get; set; }
        public IndexCreditFormModel Form { get; set; }

        public IndexCreditViewModel(ApplicationDbContext db, string? userName)
        {
            Wallets = db.Wallets.Where(x => x.UserId == userName).OrderBy(x => x.Name).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            Months = Enumerable.Range(1, 12).Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString("00") });
            Years = Enumerable.Range(2022, DateTime.UtcNow.Year - 2022 + 1).Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() });
            Form = new IndexCreditFormModel { };
        }
    }
}
