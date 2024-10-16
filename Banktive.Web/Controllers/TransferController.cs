using Banktive.Web.Data;
using Banktive.Web.Models;
using Banktive.Web.Models.TransferModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Controllers
{
    [Authorize]
    public class TransferController : Controller
    {
        private ApplicationDbContext _db;

        public TransferController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(long? wallet, int? month, int? year, int page = 1, int qty = 20)
        {
            IndexTransferViewModel model = new IndexTransferViewModel(_db, User.Identity.Name);
            IEnumerable<TransferDTO> payments = _db.Payments.Include(x => x.Wallet).Include(x => x.Currency).Where(x => x.UserId == User.Identity.Name && (x.PaymentStatusId == Constants.PaymentConfirmed))
                .Select(x => new TransferDTO
                {
                    Amount = x.Amount,
                    AssetCode = x.Currency.Code,
                    Comments = x.Comments,
                    CreatedAt = x.CreatedAt,
                    DestinationAddress = x.Destination != null? x.Destination.Account : x.XRPLDestinationWallet,
                    Id = x.Id,
                    Wallet = x.Wallet.Name,
                    WalletId = x.OriginWalletId, OriginAddress = x.Wallet.Alias,
                    IsSend = true
                });
            var myWalletAddresses = _db.Wallets.Where(x => x.UserId == User.Identity.Name).Select(x => x.XRPLAddress);
            var filterTransferForWallets = _db.Payments.Where(x => myWalletAddresses.Any(y => y == x.XRPLDestinationWallet) && x.PaymentStatusId == Constants.PaymentConfirmed).Select(x => new TransferDTO
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
                IsSend = false
            });
            payments = payments.Concat(filterTransferForWallets);
            if (wallet.HasValue)
            {
                payments = payments.Where(x => x.WalletId == wallet.Value);
            }
            if (month.HasValue)
            {
                payments = payments.Where(x => x.CreatedAt.Month == month.Value);
            }
            if (year.HasValue)
            {
                payments = payments.Where(x => x.CreatedAt.Year == year.Value);
            }
            model.Payments = payments.OrderByDescending(x => x.CreatedAt).Skip(qty * (page - 1)).Take(qty);
            model.Form.Page = page;
            model.Form.Year = year;
            model.Form.Month = month;
            model.Form.WalletId = wallet;
            return View(model);
        }


        public IActionResult Detail(Guid id)
        {
            DetailTransferViewModel model = new DetailTransferViewModel(_db, id);
            return View(model);
        }
    }
}
