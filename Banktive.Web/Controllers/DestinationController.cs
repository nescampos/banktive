using Banktive.Web.Data;
using Banktive.Web.Models.DestinationModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banktive.Web.Controllers
{
    [Authorize]
    public class DestinationController : Controller
    {
        private ApplicationDbContext _db;

        public DestinationController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(IndexDestinationFormModel Form)
        {
            IndexDestinationViewModel model = new IndexDestinationViewModel(_db, User.Identity.Name);
            if(!string.IsNullOrWhiteSpace(Form.Name))
            {
                model.Destinations = model.Destinations.Where(x => x.Name.Contains(Form.Name));
            }
            if (!string.IsNullOrWhiteSpace(Form.Email))
            {
                model.Destinations = model.Destinations.Where(x => x.Email.Contains(Form.Email));
            }
            if (!string.IsNullOrWhiteSpace(Form.Account))
            {
                model.Destinations = model.Destinations.Where(x => x.Account.Contains(Form.Account));
            }
            model.Form = Form;
            return View(model);
        }

        public IActionResult Add()
        {
            AddDestinationViewModel model = new AddDestinationViewModel(_db, User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(AddDestinationFormModel Form)
        {
            if(Form.Account!= null)
            {
                var exists = _db.Destinations.Any(x => x.UserId == User.Identity.Name && x.Account == Form.Account);
                if(exists)
                {
                    ModelState.AddModelError("Form.Account", "This account was already added.");
                }
            }
            if(!ModelState.IsValid)
            {
                AddDestinationViewModel model = new AddDestinationViewModel(_db, User.Identity.Name);
                model.Form = Form;
                return View(model);
            }

            Destination destination = new Destination
            {
                 Account = Form.Account, CreatedAt = DateTime.UtcNow, Email = Form.Email, Id = Guid.NewGuid(),
                 LastUpdatedAt = DateTime.UtcNow, Name = Form.Name, UserId = User.Identity.Name
            };
            _db.Destinations.Add(destination);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid id)
        {
            EditDestinationViewModel model = new EditDestinationViewModel(_db, id);
            if(model.Destination == null || model.Destination.UserId != User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditDestinationFormModel Form)
        {
            EditDestinationViewModel model = new EditDestinationViewModel(_db, Form.Id);
            if (model.Destination == null || model.Destination.UserId != User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                model.Form = Form;
                return View(model);
            }
            Destination destination = _db.Destinations.SingleOrDefault(x => x.Id == Form.Id);
            destination.Name = Form.Name;
            destination.LastUpdatedAt = DateTime.UtcNow;
            destination.Email = Form.Email;
            //destination.Account = Form.Account;

            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            Destination destination = _db.Destinations.SingleOrDefault(x => x.Id == id);
            if(destination == null || destination.UserId != User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            _db.Destinations.Remove(destination);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Detail(Guid id)
        {
            DetailDestinationViewModel model = new DetailDestinationViewModel(_db, id);
            if (model.Destination == null || model.Destination.UserId != User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
