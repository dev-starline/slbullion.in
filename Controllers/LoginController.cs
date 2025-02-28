using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SL_Bullion.DAL;
using SL_Bullion.Models;

namespace SL_Bullion.Controllers
{
    public class LoginController : Controller
    {
        private readonly BullionDbContext _context;
        private readonly IToastNotification _alert;
        public LoginController(BullionDbContext context, IToastNotification alert)
        {
            _context = context;
            _alert = alert;
        }
        public IActionResult List()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("List");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("userName,password")] Master login)
        {
            if (login.userName == null || login.password == null)
            {
                _alert.AddErrorToastMessage("Username or password is null.");
            }
            if (!_context.tblMaster.Any(m => m.userName == login.userName && m.isActive))
            {
                _alert.AddErrorToastMessage("Your account is inactive. Please contact the administrator.");
                return RedirectToAction("List", "Login");
            }
            var data = await _context.tblMaster
               .FirstOrDefaultAsync(m => m.userName == login.userName && m.password == login.password);
            if (data != null)
            {
                HttpContext.Session.SetString("role", "admin");
                HttpContext.Session.SetInt32("clientId", data.id);
                HttpContext.Session.SetString("userName", data.userName);
                HttpContext.Session.SetString("adminData", data.isCoin+"|"+ data.isJewellery + "|" + data.isKyc + "|" + data.isOtr + "|" + data.isUpdate + "|" + data.isBank + "|" + data.isFeedback + "|" + data.isClientRate);
                return RedirectToAction("List", "Symbol");
            }
            else
            {
                _alert.AddErrorToastMessage("Invalid username or password.");
            }

            return RedirectToAction("List", "Login");
        }
    }
}
