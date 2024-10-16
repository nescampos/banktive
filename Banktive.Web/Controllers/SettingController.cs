using Banktive.Web.Data;
using Banktive.Web.Models.SettingModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banktive.Web.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {
        private ApplicationDbContext _db;

        public SettingController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IndexSettingViewModel model = new IndexSettingViewModel(_db, User.Identity.Name);
            return View(model);
        }

        public IActionResult UpdateProfile()
        {
            UpdateProfileViewModel model = new UpdateProfileViewModel(_db, User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateProfile(UpdateProfileFormModel Form)
        {
            UpdateProfileViewModel model = new UpdateProfileViewModel(_db, User.Identity.Name);
            if (!ModelState.IsValid)
            {
                model.Form = Form;
                return View(model);
            }

            Profile profile = _db.Profiles.SingleOrDefault(x => x.UserId == User.Identity.Name);
            if(profile == null)
            {
                profile = new Profile();
                profile.UserId = User.Identity.Name;
                profile.FirstName = Form.FirstName;
                profile.LastName = Form.LastName;
                profile.AddressLine1 = Form.AddressLine1;
                profile.AddressLine2 = Form.AddressLine2;
                profile.UpdatedAt = DateTime.UtcNow;
                profile.City = Form.City;
                profile.CountryId = Form.CountryId.Value;
                profile.PostalCode = Form.PostalCode;
                profile.TaxId = Form.TaxId;
                _db.Profiles.Add(profile);
            }
            else
            {
                profile.AddressLine1 = Form.AddressLine1;
                profile.AddressLine2 = Form.AddressLine2;
                profile.UpdatedAt = DateTime.UtcNow;
                profile.City = Form.City;
                profile.CountryId = Form.CountryId.Value;
                profile.PostalCode = Form.PostalCode;
                profile.TaxId = Form.TaxId;
            }
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
