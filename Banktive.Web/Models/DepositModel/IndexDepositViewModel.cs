using Banktive.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Banktive.Web.Models.DepositModel
{
    public class IndexDepositViewModel
    {
        public IEnumerable<DepositDTO> Deposits { get; set; }
        public IndexDepositFormModel Form { get; set; }
        public IEnumerable<SelectListItem> Months { get; set; }
        public IEnumerable<SelectListItem> Years { get; set; }
        public IEnumerable<SelectListItem> Wallets { get; set; }
        public int Page { get; set; }

        public IndexDepositViewModel(ApplicationDbContext db, string? userName)
        {
            Form = new IndexDepositFormModel();
            Wallets = db.Wallets.Where(x => x.UserId == userName).OrderBy(x => x.Name).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            Months = Enumerable.Range(1, 12).Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString("00") });
            Years = Enumerable.Range(2022, DateTime.UtcNow.Year - 2022 + 1).Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() });
        }
    }
}
