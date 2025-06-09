using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlinePaymentSite.Services.DTOs.Payment;
using OnlinePaymentSite.Services.Interfaces;
using OnlinePaymentSite.Web.ViewModels.Payment;

namespace OnlinePaymentSite.Web.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IAccountService _accountService;

        public PaymentController(IPaymentService paymentService, IAccountService accountService)
        {
            _paymentService = paymentService;
            _accountService = accountService;
        }

        public async Task<IActionResult> Create()
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;
            var accounts = await _accountService.GetAccountsForUserAsync(userId);
            ViewBag.Accounts = accounts.Accounts;
            return View(new CreatePaymentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;
            var accounts = await _accountService.GetAccountsForUserAsync(userId);
            ViewBag.Accounts = accounts.Accounts;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _paymentService.CreatePaymentAsync(new CreatePaymentRequest
            {
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId,
                Amount = model.Amount,
                Reason = model.Reason
            });

            if (result.Success)
            {
                TempData["Success"] = "Payment created successfully.";
                return RedirectToAction("History");
            }

            ModelState.AddModelError("", result.ErrorMessage ?? "Failed to create payment.");
            return View(model);
        }

        public async Task<IActionResult> History(int accountId = 0)
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;
            var accounts = await _accountService.GetAccountsForUserAsync(userId);
            ViewBag.Accounts = accounts.Accounts;

            var payments = accountId == 0
                ? new GetAllPaymentsResponse { Payments = new List<PaymentInfo>(), TotalCount = 0 }
                : await _paymentService.GetPaymentsForAccountAsync(accountId);

            return View(new PaymentHistoryViewModel
            {
                SelectedAccountId = accountId,
                Payments = payments.Payments
            });
        }
    }
}
