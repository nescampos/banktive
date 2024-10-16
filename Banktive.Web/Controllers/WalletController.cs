using Banktive.Web.Data;
using Banktive.Web.Models.WalletModel;
using Banktive.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banktive.Web.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        private ApplicationDbContext _db;
        private XRPLService _XRPLService;

        public WalletController(ApplicationDbContext db, XRPLService xrplService)
        {
            _db = db;
            _XRPLService = xrplService;
        }

        public IActionResult Index()
        {
            IndexWalletViewModel model = new IndexWalletViewModel(_db, User.Identity.Name);
            if(model.Profile == null)
            {
                return RedirectToAction("Index", "Setting");
            }
            return View(model);
        }

        public IActionResult Create()
        {
            CreateWalletViewModel model = new CreateWalletViewModel(_db);
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateWalletFormModel Form)
        {
            CreateWalletViewModel model = new CreateWalletViewModel(_db);
            if (!string.IsNullOrWhiteSpace(Form.Alias))
            {
                string alias = Form.Alias.ToLower();
                bool existAlias = _db.Wallets.Any(x => x.Alias == alias);
                if (existAlias)
                {
                    ModelState.AddModelError("Form.Alias", "The alias is already occupied by another account");
                }
            }
            if (!ModelState.IsValid)
            {
                model.Form = Form;
                return View(model);
            }

            Wallet wallet = new Wallet
            {
                 Alias = Form.Alias, CreatedAt = DateTime.UtcNow, CurrencyId = Form.CurrencyId.Value,
                 XRPLAddress = Form.XRPLAddress, Description = Form.Description, Name = Form.Name,
                 XRPLSeed = Form.XRPLSeed, UserId = User.Identity.Name
            };
            _db.Wallets.Add(wallet);
            _db.SaveChanges();
            return RedirectToAction("View", new { id = wallet.Id });
        }

        public IActionResult View(long id)
        {
            ViewWalletViewModel model = new ViewWalletViewModel(_db, id);
            if(model.Wallet == null || model.Wallet.UserId != User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Update(long id)
        {
            UpdateWalletViewModel model = new UpdateWalletViewModel(_db, id);
            if (model.Wallet == null || model.Wallet.UserId != User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(UpdateWalletFormModel Form)
        {
            UpdateWalletViewModel model = new UpdateWalletViewModel(_db, Form.Id);
            if (model.Wallet == null || model.Wallet.UserId != User.Identity.Name)
            {
                model.Form = Form;
                return View(model);
            }
            Wallet wallet = _db.Wallets.SingleOrDefault(x => x.Id == Form.Id);
            wallet.Name = Form.Name;
            wallet.Description = Form.Description;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetNativeBalance(long id)
        {
            Wallet wallet = _db.Wallets.SingleOrDefault(x => x.Id == id);
            if (wallet== null || wallet.UserId != User.Identity.Name)
            {
                return Json(null);
            }
            var nativeBalance = await _XRPLService.GetNativeBalance("wss://s.altnet.rippletest.net:51233", wallet.XRPLAddress);
            if (nativeBalance == null)
            {
                return Json(null);
            }
            return Json(nativeBalance);
        }
    }
}
