using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlinePaymentSite.Services.Interfaces;

namespace OnlinePaymentSite.Web.Controllers
{
    [Authorize]
    public class BankAccountController : Controller
    {
        private readonly IAccountService _accountService;

        public BankAccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;
            var accounts = await _accountService.GetAccountsForUserAsync(userId);
            return View(accounts);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var account = await _accountService.GetByIdAsync(id);
                return View(account);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
