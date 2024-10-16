using Banktive.Web.Data;
using Banktive.Web.Models;
using Banktive.Web.Models.PaymentModel;
using Banktive.Web.Services;
using Banktive.Web.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private ApplicationDbContext _db;
        private UserManager<IdentityUser> _userManager;
        private XRPLService _XRPLService;

        public PaymentController(ApplicationDbContext db, UserManager<IdentityUser> userManager, XRPLService xrplService)
        {
            _db = db;
            _userManager = userManager;
            _XRPLService = xrplService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SelectWallet()
        {
            SelectWalletViewModel model = new SelectWalletViewModel(_db, User.Identity.Name);
            return View(model);
        }

        public IActionResult Send(long? id)
        {
            SendRegularPaymentViewModel model = new SendRegularPaymentViewModel(_db, id, User.Identity.Name);
            if(model.Wallet == null || model.Wallet.UserId != User.Identity.Name)
            {
                return RedirectToAction("SelectWallet");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Send(SendRegularPaymentFormModel Form)
        {
            if(!ModelState.IsValid)
            {
                SendRegularPaymentViewModel model = new SendRegularPaymentViewModel(_db, Form.OriginWalletId, User.Identity.Name);
                model.Form = Form;
                return View(model);
            }

            Destination destination = _db.Destinations.SingleOrDefault(x => x.Id == Form.DestinationId.Value);
            string XRPLDestination = destination.Account;
            Wallet existingWalletForDestination = _db.Wallets.FirstOrDefault(x => x.Alias == XRPLDestination || x.Id.ToString() == XRPLDestination);
            if(existingWalletForDestination != null)
            {
                XRPLDestination = existingWalletForDestination.XRPLAddress;
            }

            Payment payment = new Payment
            {
                 Amount = Form.Amount.Value, Comments = Form.Comments, PaymentStatusId = Constants.PaymentCreated, PaymentTypeId = Constants.RegularPayment,
                 CreatedAt = DateTime.UtcNow, CurrencyId = Constants.Currency_XRP, DestinationId = Form.DestinationId.Value,
                 XRPLDestinationWallet = XRPLDestination, Id = Guid.NewGuid(), OriginWalletId = Form.OriginWalletId.Value, UserId = User.Identity.Name
            };
            _db.Payments.Add(payment);
            _db.SaveChanges();

            return RedirectToAction("Approve", new { id = payment.Id });
        }

        public IActionResult Approve(Guid id)
        {
            ApproveRegularPaymentViewModel model = new ApproveRegularPaymentViewModel(_db, id);
            if(model.Payment == null || model.Payment.UserId != User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(ApproveRegularPaymentFormModel Form)
        {
            ApproveRegularPaymentViewModel model = new ApproveRegularPaymentViewModel(_db, Form.PaymentId);
            if (model.Payment == null || model.Payment.UserId != User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            //if (!string.IsNullOrEmpty(Form.Password))
            //{
            //    var passwordValidator = new PasswordValidator<IdentityUser>();
            //    var result = await passwordValidator.ValidateAsync(_userManager, new IdentityUser(User.Identity.Name), Form.Password);

            //    if (result.Succeeded)
            //    {
            //        // Valid Password
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("Form.Password", "The password is not correct.");
            //    }
            //}
            if (!ModelState.IsValid)
            {
                
                model.Form = Form;
                return View(model);
            }
            Payment payment = _db.Payments.Include(x => x.Currency).Include(x => x.Destination)
                .Include(x => x.PaymentStatus).Include(x => x.PaymentType).SingleOrDefault(x => x.Id == Form.PaymentId);

            if(!string.IsNullOrEmpty(Form.Cancelled))
            {
                payment.PaymentStatusId = Constants.PaymentCancelled;
                payment.ConfirmationAt = DateTime.UtcNow;
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            NativePaymentResult resultPayment = await _XRPLService.CreatePayment("wss://s.altnet.rippletest.net:51233", payment.XRPLDestinationWallet, payment.Amount,
                payment.Wallet.XRPLAddress, payment.Wallet.XRPLSeed);
            if(resultPayment.Successful)
            {
                payment.PaymentStatusId = Constants.PaymentConfirmed;
                payment.Fee = resultPayment.FeeAmount;
            }
            else
            {
                payment.PaymentStatusId = Constants.PaymentRejected;
            }
            payment.ConfirmationAt = DateTime.UtcNow;
            _db.SaveChanges();

            return RedirectToAction("Detail","Transfer", new { id = payment.Id });
        }
    }
}
