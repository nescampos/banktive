using Banktive.Web.Data;
using Banktive.Web.Models;
using Banktive.Web.Models.ServiceModel;
using Banktive.Web.Services;
using Banktive.Web.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        private ApplicationDbContext _db;
        private XRPLService _XRPLService;

        public ServiceController(ApplicationDbContext db, XRPLService xrplService)
        {
            _db = db;
            _XRPLService = xrplService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registered()
        {
            MyRegisteredViewModel model = new MyRegisteredViewModel(_db, User.Identity.Name);
            return View(model);
        }

        public IActionResult Register()
        {
            RegisterServiceViewModel model = new RegisterServiceViewModel(_db, User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterServiceFormModel Form)
        {
            if(!ModelState.IsValid)
            {
                RegisterServiceViewModel model = new RegisterServiceViewModel(_db, User.Identity.Name);
                model.Form = Form;
                return View(model);
            }

            Service service = new Service()
            {
                Name = Form.Name,
                CreatedAt = DateTime.UtcNow,
                UserId = User.Identity.Name,
                WalletId = Form.WalletId.Value, ServiceTypeId = Form.ServiceTypeId.Value
            };
            _db.Services.Add(service);
            _db.SaveChanges();
            return RedirectToAction("ServiceDetail", new { id = service.Id });
        }

        public IActionResult ServiceDetail(long id)
        {
            MyServiceViewModel model = new MyServiceViewModel(_db, id);
            if (model.Service == null || model.Service.UserId != User.Identity.Name)
            {
                return RedirectToAction("Registered");
            }
            return View(model);
        }

        public IActionResult ServiceAccounts(long id)
        {
            ServiceAccountsViewModel model = new ServiceAccountsViewModel(_db, id);
            if (model.Service == null || model.Service.UserId != User.Identity.Name)
            {
                return RedirectToAction("Registered");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult ServiceAccounts(ServiceAccountsFormModel Form)
        {
            ServiceAccountsViewModel model = new ServiceAccountsViewModel(_db, Form.Id);
            if(!string.IsNullOrEmpty(Form.CustomerId))
            {
                bool exists = _db.ServiceAccounts.Any(x => x.ServiceId == Form.Id && x.CustomerId == Form.CustomerId);
                if(exists)
                {
                    ModelState.AddModelError("Form.CustomerId", "This account already exists.");
                }
            }
            if (!ModelState.IsValid)
            {
                model.Form = Form;
                return View(model);
            }
            ServiceAccount serviceAccount = new ServiceAccount
            {
                 CustomerId = Form.CustomerId, CreatedAt = DateTime.UtcNow, ServiceId = Form.Id
            };
            _db.ServiceAccounts.Add(serviceAccount);
            _db.SaveChanges();
            return RedirectToAction("ServiceAccounts", new { id = Form.Id });
        }

        public IActionResult ServiceAccountInfo(long id)
        {
            ServiceAccountInfoViewModel model = new ServiceAccountInfoViewModel(_db, id);
            return View(model);
        }

        [HttpPost]
        public IActionResult ServiceAccountInfo(ServiceAccountInfoFormModel Form)
        {
            ServiceAccountInfoViewModel model = new ServiceAccountInfoViewModel(_db, Form.ServiceAccountId);
            if(Form.DueDate.HasValue && Form.DueDate.Value.Date < DateTime.UtcNow.Date)
            {
                ModelState.AddModelError("Form.DueDate", "The date must be today or later.");
            }
            if(!ModelState.IsValid)
            {
                model.Form = Form;
                return View(model);
            }
            ServiceAccountDetail detail = new ServiceAccountDetail
            {
                 ServiceAccountId = Form.ServiceAccountId, Amount = Form.Amount.Value, CreatedAt = DateTime.UtcNow,
                 CurrencyId = Constants.Currency_XRP, DueDate = Form.DueDate.Value, Id = Guid.NewGuid(), Paid = false
            };
            _db.ServiceAccountDetails.Add(detail);
            _db.SaveChanges();
            return RedirectToAction("ServiceAccountInfo", new { id = Form.ServiceAccountId });
        }


        public IActionResult Subscriptions()
        {
            MySubscriptionsViewModel model = new MySubscriptionsViewModel(_db, User.Identity.Name);
            return View(model);
        }

        public IActionResult Subscribe()
        {
            SubscribeServiceViewModel model = new SubscribeServiceViewModel(_db, User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        public IActionResult Subscribe(SubscribeServiceFormModel Form)
        {
            SubscribeServiceViewModel model = new SubscribeServiceViewModel(_db, User.Identity.Name);
            if(Form.ServiceTypeId.HasValue && Form.CustomerId != null && Form.ServiceId.HasValue)
            {
                bool existService = _db.ServiceAccounts.Include(x => x.Service).Any(x => x.Service.ServiceTypeId == Form.ServiceTypeId && x.CustomerId == Form.CustomerId && x.ServiceId == Form.ServiceId);
                if(!existService)
                {
                    ModelState.AddModelError("Form.CustomerId", "There is no registered account for this customer and service.");
                }
            }
            if (!ModelState.IsValid)
            {
                model.Form = Form;
                return View(model);
            }
            
            Subscription subscription = new Subscription
            {
                 CreatedAt = DateTime.UtcNow, CustomerId = Form.CustomerId, Id = Guid.NewGuid(),
                 ServiceId = Form.ServiceId.Value, ServiceName = Form.ServiceName, ServiceTypeId = Form.ServiceTypeId.Value,
                 UserId = User.Identity.Name
            };
            _db.Subscriptions.Add(subscription);
            _db.SaveChanges();

            return RedirectToAction("Subscriptions");
        }

        public IActionResult CheckSubscription(Guid id)
        {
            CheckSubscriptionViewModel model = new CheckSubscriptionViewModel(_db, id, User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CheckSubscription(CheckSubscriptionFormModel Form)
        {
            CheckSubscriptionViewModel model = new CheckSubscriptionViewModel(_db, Form.SubscriptionId, User.Identity.Name);
            if (!ModelState.IsValid)
            {
                
                model.Form = Form;
                return View(model);
            }

            Wallet walletForPayment = _db.Wallets.SingleOrDefault(x => x.Id == Form.WalletId.Value);
            string xrplAddress = model.PendingAccount.ServiceAccount.Service.Wallet.XRPLAddress;
            string serviceName = model.PendingAccount.ServiceAccount.Service.Name;
            Payment payment = new Payment
            {
                Amount = model.PendingAccount.Amount,
                PaymentStatusId = Constants.PaymentCreated,
                PaymentTypeId = Constants.ServicePayment,
                CreatedAt = DateTime.UtcNow,
                CurrencyId = Constants.Currency_XRP,
                //DestinationId = model.PendingAccount.ServiceAccount.Service.Wallet.XRPLAddress,
                XRPLDestinationWallet = xrplAddress,
                Id = Guid.NewGuid(),
                OriginWalletId = Form.WalletId.Value,
                UserId = User.Identity.Name,
                Comments = "Service payment for "+ serviceName
            };


            NativePaymentResult resultPayment = await _XRPLService.CreatePayment("wss://s.altnet.rippletest.net:51233", payment.XRPLDestinationWallet, payment.Amount,
                walletForPayment.XRPLAddress, walletForPayment.XRPLSeed);
            if (resultPayment.Successful)
            {
                payment.PaymentStatusId = Constants.PaymentConfirmed;
                payment.Fee = resultPayment.FeeAmount;
                model.PendingAccount.Paid = true;
            }
            else
            {
                payment.PaymentStatusId = Constants.PaymentRejected;
            }
            payment.ConfirmationAt = DateTime.UtcNow;
            _db.Payments.Add(payment);
            _db.SaveChanges();

            return RedirectToAction("CheckSubscription", new { id = Form.SubscriptionId });
        }
    }
}
