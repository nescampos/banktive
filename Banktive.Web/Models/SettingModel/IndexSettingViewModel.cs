using Banktive.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Models.SettingModel
{
    public class IndexSettingViewModel
    {
        public Profile Profile { get; set; }

        public IndexSettingViewModel(ApplicationDbContext db, string? username)
        {
            Profile = db.Profiles.Include(x => x.Country).SingleOrDefault(x => x.UserId == username);
        }
    }
}
