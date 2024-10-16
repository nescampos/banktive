using Banktive.Web.Data;
using Banktive.Web.Models;
using Banktive.Web.Models.DepositModel;
using Banktive.Web.Services;
using Banktive.Web.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Controllers
{
    [Authorize]
    public class DepositController : Controller
    {
        private ApplicationDbContext _db;
        private XRPLService _XRPLService;

        public DepositController(ApplicationDbContext db, XRPLService xrplService)
        {
            _db = db;
            _XRPLService = xrplService;
        }

        public IActionResult Index(long? wallet, int? month, int? year, int page = 1, int qty = 20)
        {
            IndexDepositViewModel model = new IndexDepositViewModel(_db, User.Identity.Name);
            IEnumerable<DepositDTO> deposits = _db.Deposits.Include(x => x.Wallet).Include(x => x.Currency).Where(x => x.UserId == User.Identity.Name && (x.DepositStatusId == Constants.EscrowConfirmed || x.DepositStatusId == Constants.EscrowCashed || x.DepositStatusId == Constants.EscrowExpired))
                .Select(x => new DepositDTO
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
                    IsSend = true,
                    DateToCash = x.DateToCash
                });
            var myWalletAddresses = _db.Wallets.Where(x => x.UserId == User.Identity.Name).Select(x => x.XRPLAddress);
            var filterDepositsForWallets = _db.Deposits.Where(x => myWalletAddresses.Any(y => y == x.XRPLDestinationWallet) && (x.DepositStatusId == Constants.EscrowConfirmed || x.DepositStatusId == Constants.EscrowCashed || x.DepositStatusId == Constants.EscrowExpired)).Select(x => new DepositDTO
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
            deposits = deposits.Concat(filterDepositsForWallets);
            if (wallet.HasValue)
            {
                deposits = deposits.Where(x => x.WalletId == wallet.Value);
            }
            if (month.HasValue)
            {
                deposits = deposits.Where(x => x.CreatedAt.Month == month.Value);
            }
            if (year.HasValue)
            {
                deposits = deposits.Where(x => x.CreatedAt.Year == year.Value);
            }
            model.Deposits = deposits.OrderByDescending(x => x.CreatedAt).Skip(qty * (page - 1)).Take(qty);
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

        public IActionResult CreateTimeDeposit(long? id)
        {
            CreateTimeDepositViewModel model = new CreateTimeDepositViewModel(_db, id, User.Identity.Name);
            if (model.Wallet == null || model.Wallet.UserId != User.Identity.Name)
            {
                return RedirectToAction("SelectWallet");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateTimeDeposit(CreateTimeDepositFormModel Form)
        {
            CreateTimeDepositViewModel model = new CreateTimeDepositViewModel(_db, Form.OriginWalletId, User.Identity.Name);
            if (Form.DateToCash.HasValue)
            {
                if (Form.DateToCash.Value.Date <= DateTime.UtcNow.Date)
                {
                    ModelState.AddModelError("Form.DateToCash", "The date must be in the future (from tomorrow).");
                }
            }
            if(!ModelState.IsValid)
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

            Deposit deposit = new Deposit()
            {
                Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Amount = Form.Amount.Value,
                Comments = Form.Comments, CurrencyId = Constants.Currency_XRP, DateToCash = Form.DateToCash,
                DepositStatusId = Constants.EscrowCreated, DepositTypeId = Constants.TimeEscrow, DestinationId = Form.DestinationId,
                XRPLDestinationWallet = XRPLDestination, OriginWalletId = Form.OriginWalletId.Value, UserId = User.Identity.Name
            };
            _db.Deposits.Add(deposit);
            _db.SaveChanges();

            return RedirectToAction("ConfirmTimeDeposit", new { id = deposit.Id });
        }

        public IActionResult ConfirmTimeDeposit(Guid id)
        {
            ConfirmTimeDepositViewModel model = new ConfirmTimeDepositViewModel(_db, id);
            if (model.Deposit == null || model.Deposit.UserId != User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmTimeDeposit(ConfirmTimeDepositFormModel Form)
        {
            ConfirmTimeDepositViewModel model = new ConfirmTimeDepositViewModel(_db, Form.Id);
            if (!ModelState.IsValid)
            {
                model.Form = Form;
                return View(model);
            }

            Deposit deposit = _db.Deposits.SingleOrDefault(x => x.Id == Form.Id);

            if (!string.IsNullOrEmpty(Form.Cancelled))
            {
                deposit.DepositStatusId = Constants.EscrowCancelled;
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            XRPLCreateEscrowResult resultDeposit = await _XRPLService.CreateEscrow("wss://s.altnet.rippletest.net:51233", deposit.XRPLDestinationWallet, deposit.Amount,
                deposit.Wallet.XRPLAddress, deposit.Wallet.XRPLSeed, deposit.DateToCash.Value);

            if (resultDeposit.Successful)
            {
                deposit.DepositStatusId = Constants.EscrowConfirmed;
                deposit.Fee = resultDeposit.FeeAmount;
                deposit.XRPLSequence = (int?)resultDeposit.Sequence;
                deposit.TransactionHash = resultDeposit.Hash;
            }
            else
            {
                deposit.DepositStatusId = Constants.EscrowRejected;
            }
            deposit.ConfirmationAt = DateTime.UtcNow;
            _db.SaveChanges();

            return RedirectToAction("ViewTimeDeposit", new { id = Form.Id });
        }

        public IActionResult ViewTimeDeposit(Guid id)
        {
            ViewTimeDepositViewModel model = new ViewTimeDepositViewModel(_db, id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CashTimeDeposit(Guid id, long idWalletToCash)
        {
            Deposit deposit = _db.Deposits.Include(x => x.Wallet).SingleOrDefault(x => x.Id == id);
            Wallet walletToCash = _db.Wallets.SingleOrDefault(x => x.Id == idWalletToCash);
            if (walletToCash == null || walletToCash.UserId != User.Identity.Name)
            {
                return RedirectToAction("Index");
            }

            XRPLCreateEscrowResult resultDeposit = await _XRPLService.FinishEscrow("wss://s.altnet.rippletest.net:51233",
                walletToCash.XRPLAddress, walletToCash.XRPLSeed, deposit.Wallet.XRPLAddress, (uint)deposit.XRPLSequence.Value);
            if(resultDeposit != null && resultDeposit.Successful)
            {
                Payment payment = new Payment
                {
                    Amount = deposit.Amount,
                    Comments = deposit.Comments,
                    PaymentStatusId = Constants.PaymentConfirmed,
                    PaymentTypeId = Constants.TimeDepositPayment,
                    CreatedAt = DateTime.UtcNow,
                    CurrencyId = Constants.Currency_XRP,
                    DestinationId = deposit.DestinationId,
                    XRPLDestinationWallet = deposit.XRPLDestinationWallet,
                    Id = Guid.NewGuid(),
                    OriginWalletId = deposit.OriginWalletId,
                    UserId = deposit.UserId,
                    ConfirmationAt = DateTime.UtcNow,
                    Fee = resultDeposit.FeeAmount
                };
                _db.Payments.Add(payment);

                deposit.DepositStatusId = Constants.EscrowCashed;
            }

            
            _db.SaveChanges();
            return RedirectToAction("ViewTimeDeposit", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> CancelTimeDeposit(Guid id)
        {
            Deposit deposit = _db.Deposits.Include(x => x.Wallet).SingleOrDefault(x => x.Id == id);

            XRPLCreateEscrowResult resultDeposit = await _XRPLService.CancelEscrow("wss://s.altnet.rippletest.net:51233",
                deposit.Wallet.XRPLAddress, deposit.Wallet.XRPLSeed, deposit.Wallet.XRPLAddress, (uint)deposit.XRPLSequence.Value);
            if (resultDeposit != null && resultDeposit.Successful)
            {

                deposit.DepositStatusId = Constants.EscrowExpired;
            }


            _db.SaveChanges();
            return RedirectToAction("ViewTimeDeposit", new { id = id });
        }
    }
}
