using Banktive.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Banktive.Web.Models.SettingModel
{
    public class UpdateProfileViewModel
    {
        public UpdateProfileFormModel Form { get; set; }
        public Profile Profile { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }

        public UpdateProfileViewModel(ApplicationDbContext db, string? username)
        {
            Countries = db.Countries.Where(x => x.Enabled).OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            Profile = db.Profiles.SingleOrDefault(x => x.UserId == username);
            if(Profile != null)
            {
                Form = new UpdateProfileFormModel
                {
                    CountryId = Profile.CountryId, AddressLine1 = Profile.AddressLine1,
                    AddressLine2 = Profile.AddressLine2,City = Profile.City,FirstName = Profile.FirstName,
                    LastName = Profile.LastName, PostalCode = Profile.PostalCode, TaxId = Profile.TaxId
                };
            }
        }

        


    }
}
