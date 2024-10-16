using Banktive.Web.Data;
using Banktive.Web.Models;
using Banktive.Web.Models.CreditModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Controllers
{
    [Authorize]
    public class CreditController : Controller
    {

        private ApplicationDbContext _db;

        public CreditController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(long? wallet, int? month, int? year, int page = 1, int qty = 20)
        {
            IndexCreditViewModel model = new IndexCreditViewModel(_db, User.Identity.Name);
            IEnumerable<CreditDTO> checks = _db.Checks.Include(x => x.Wallet).Include(x => x.Currency).Where(x => x.UserId == User.Identity.Name && (x.CheckStatusId == Constants.CheckConfirmed || x.CheckStatusId == Constants.CheckCashed))
                .Select(x => new CreditDTO
                {
                    Amount = x.Amount,
                    AssetCode = x.Currency.Code,
                    Comments = x.Comments,
                    CreatedAt = x.CreatedAt,
                    DestinationAddress = x.Destination != null ? x.Destination.Account : x.XRPLDestinationWallet,
                    Id = x.Id,
                    Wallet = x.Wallet.Name,
                    WalletId = x.OriginWalletId,
                    OriginAddress = x.Wallet.Alias,
                    IsSend = true, DateToCash = x.DateToCash
                });
            var myWalletAddresses = _db.Wallets.Where(x => x.UserId == User.Identity.Name).Select(x => x.XRPLAddress);
            var filterTransferForWallets = _db.Checks.Where(x => myWalletAddresses.Any(y => y == x.XRPLDestinationWallet) && (x.CheckStatusId == Constants.CheckConfirmed || x.CheckStatusId == Constants.CheckCashed)).Select(x => new CreditDTO
            {
                Amount = x.Amount,
                AssetCode = x.Currency.Code,
                Comments = x.Comments,
                CreatedAt = x.CreatedAt,
                DestinationAddress = x.Destination.Account,
                Id = x.Id,
                Wallet = x.Wallet.Name,
                WalletId = x.OriginWalletId,
                OriginAddress = x.Wallet.Alias,
                IsSend = false,
                DateToCash = x.DateToCash
            });
            checks = checks.Concat(filterTransferForWallets);
            if (wallet.HasValue)
            {
                checks = checks.Where(x => x.WalletId == wallet.Value);
            }
            if (month.HasValue)
            {
                checks = checks.Where(x => x.CreatedAt.Month == month.Value);
            }
            if (year.HasValue)
            {
                checks = checks.Where(x => x.CreatedAt.Year == year.Value);
            }
            model.Checks = checks.OrderByDescending(x => x.CreatedAt).Skip(qty * (page - 1)).Take(qty);
            model.Form.Page = page;
            model.Form.Year = year;
            model.Form.Month = month;
            model.Form.WalletId = wallet;
            return View(model);
        }

        public IActionResult SelectWallet()
        {
            SelectWalletViewModel model = new SelectWalletViewModel(_db, User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        public IActionResult EnableWallet(long id)
        {
            Wallet wallet = _db.Wallets.SingleOrDefault(x => x.Id == id);
            if (wallet == null || wallet.UserId != User.Identity.Name)
            {
                return RedirectToAction("SelectWallet");
            }
            CreditWallet creditWallet = _db.CreditWallets.SingleOrDefault(x => x.Id == id);
            if(creditWallet == null)
            {
                creditWallet = new CreditWallet()
                {
                    Id = id,
                    CreatedAt = DateTime.UtcNow,
                    Enabled = true,
                    UsedAmount = 0,
                    TotalAmount = 10000, UserId = User.Identity.Name
                };
                _db.CreditWallets.Add(creditWallet);
            }
            else
            {
                if(!creditWallet.Enabled)
                {
                    creditWallet.Enabled = true;
                }
                else
                {
                    if (creditWallet.UsedAmount == 0)
                    {
                        creditWallet.Enabled = false;
                    }
                }
            }
            _db.SaveChanges();
            return RedirectToAction("SelectWallet");
        }

        public IActionResult PayWithCheck(long? id)
        {
            PayWithCheckViewModel model = new PayWithCheckViewModel(_db, id, User.Identity.Name);
            if (model.Wallet == null || model.Wallet.UserId != User.Identity.Name)
            {
                return RedirectToAction("SelectWallet");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult PayWithCheck(PayWithCheckFormModel Form)
        {
            if(Form.Amount.HasValue)
            {
                CreditWallet creditWallet = _db.CreditWallets.SingleOrDefault(x => x.Id == Form.OriginWalletId);
                if((creditWallet.TotalAmount - creditWallet.UsedAmount) < Form.Amount.Value)
                {
                    ModelState.AddModelError("Form.Amount", "You do not have enough available balance for this transaction.");
                }
            }
            if(Form.DateToCash.HasValue)
            {
                if(Form.DateToCash.Value.Date <= DateTime.UtcNow.Date)
                {
                    ModelState.AddModelError("Form.DateToCash", "The date must be in the future (from tomorrow).");
                }
            }
            PayWithCheckViewModel model = new PayWithCheckViewModel(_db, Form.OriginWalletId, User.Identity.Name);
            if (!ModelState.IsValid)
            {
                model.Form = Form;
                return View(model);
            }

            Destination destination = _db.Destinations.SingleOrDefault(x => x.Id == Form.DestinationId.Value);
            string XRPLDestination = destination.Account;
            Wallet existingWalletForDestination = _db.Wallets.FirstOrDefault(x => x.Alias == XRPLDestination || x.Id.ToString() == XRPLDestination);
            if (existingWalletForDestination != null)
            {
                XRPLDestination = existingWalletForDestination.XRPLAddress;
            }

            Check check = new Check
            {
                Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Amount = Form.Amount.Value,
                 OriginWalletId = Form.OriginWalletId, CheckStatusId = Constants.CheckCreated, Comments = Form.Comments,
                 CurrencyId = Constants.Currency_XRP, UserId = User.Identity.Name, DestinationId = Form.DestinationId,
                 XRPLDestinationWallet = XRPLDestination, DateToCash = Form.DateToCash.Value
            };
            _db.Checks.Add(check);
            _db.SaveChanges();
            return RedirectToAction("ConfirmCheck", new { id = check.Id });
        }

        public IActionResult ConfirmCheck(Guid id)
        {
            ConfirmCheckViewModel model = new ConfirmCheckViewModel(_db, id);
            if (model.Check == null || model.Check.UserId != User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmCheck(ConfirmCheckFormModel Form)
        {
            ConfirmCheckViewModel model = new ConfirmCheckViewModel(_db, Form.Id);
            if (model.Check == null || model.Check.UserId != User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            Check check = _db.Checks.Include(x => x.Currency).Include(x => x.Destination)
                .Include(x => x.CheckStatus).SingleOrDefault(x => x.Id == Form.Id);

            if (!string.IsNullOrEmpty(Form.Cancelled))
            {
                check.CheckStatusId = Constants.CheckCancelled;
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                model.Form = Form;
                return View(model);
            }
            
            check.CheckStatusId = Constants.CheckConfirmed;
            check.CheckXRPLId = Form.CheckXRPLId;
            check.Fee = 0;

            CreditWallet creditWallet = _db.CreditWallets.SingleOrDefault(x => x.Id == check.OriginWalletId);
            creditWallet.UsedAmount += check.Amount;

            _db.SaveChanges();

            return RedirectToAction("ViewCheck", new { id = Form.Id });
        }

        public IActionResult ViewCheck(Guid id)
        {
            ViewCheckViewModel model = new ViewCheckViewModel(_db, id);
            //if (model.Check == null || model.Check.UserId != User.Identity.Name)
            //{
            //    return RedirectToAction("Index");
            //}
            return View(model);
        }

        [HttpPost]
        public IActionResult CashCheck(Guid id)
        {
            Check check = _db.Checks.SingleOrDefault(x => x.Id ==id);
            check.DateToCash = DateTime.UtcNow;
            check.CheckStatusId = Constants.CheckCashed;

            Payment payment = new Payment
            {
                Amount = check.Amount,
                Comments = check.Comments,
                PaymentStatusId = Constants.PaymentConfirmed,
                PaymentTypeId = Constants.CheckPayment,
                CreatedAt = DateTime.UtcNow,
                CurrencyId = Constants.Currency_XRP,
                DestinationId = check.DestinationId,
                XRPLDestinationWallet = check.XRPLDestinationWallet,
                Id = Guid.NewGuid(),
                OriginWalletId = check.OriginWalletId,
                UserId = check.UserId, ConfirmationAt = DateTime.UtcNow, Fee = check.Fee
            };

            _db.Payments.Add(payment);

            CreditWallet creditWallet = _db.CreditWallets.SingleOrDefault(x => x.Id == check.OriginWalletId);
            creditWallet.UsedAmount -= check.Amount;
            
            _db.SaveChanges();
            return RedirectToAction("ViewCheck", new { id });
        }
    }
}
