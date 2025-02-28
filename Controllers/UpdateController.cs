using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SL_Bullion.Constant;
using SL_Bullion.DAL;
using SL_Bullion.Models;
using Swashbuckle.Swagger;
using System.Globalization;
using System.Reflection.Metadata;

namespace SL_Bullion.Controllers
{
    public class UpdateController : Controller
    {
        private readonly BullionDbContext _context;
        private readonly IToastNotification _alert;
        private readonly ApplicationConstant _constatnt;

        public UpdateController(BullionDbContext context, IToastNotification alert, ApplicationConstant constatnt)
        {
            _context = context;
            _alert = alert;
            _constatnt = constatnt;
        }
        public async Task<IActionResult> List()
        {
            return View(await _context.tblUpdate.Where(s => s.clientId == HttpContext.Session.GetInt32("clientId") && s.modifiedDate.Date == DateTime.Today).OrderByDescending(s => s.modifiedDate).ToListAsync());
        }
        
        public async Task<IActionResult> search(string fromDate, string toDate)
        {
            DateTime fromDateValue;
            DateTime toDateValue;
            string dateFormat = "yyyy-MM-dd";
            if (!DateTime.TryParseExact(fromDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDateValue) ||
            !DateTime.TryParseExact(toDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out toDateValue))
            {
                _alert.AddSuccessToastMessage("Invalid date format.");
                return RedirectToAction(nameof(List));
            }
            toDateValue = toDateValue.Date.Add(new TimeSpan(23, 59, 59));
            var update = await _context.tblUpdate.Where(s => s.clientId == HttpContext.Session.GetInt32("clientId") && s.modifiedDate.Date >= fromDateValue.Date &&
                        s.modifiedDate.Date <= toDateValue.Date).OrderByDescending(s => s.modifiedDate).ToListAsync();

            return View("List", update);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,clientId,title,message,description,modifiedDate")] Update update)
        {
            if (ModelState.IsValid)
            {
                update.clientId = HttpContext.Session.GetInt32("clientId").GetValueOrDefault();
                _context.Add(update);
                await _context.SaveChangesAsync();
                _constatnt.pushAlert(update.clientId,update.title,update.message);
                _alert.AddSuccessToastMessage("Update created.");
                return RedirectToAction(nameof(List));
            }
            return View(update);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var update = await _context.tblUpdate.FindAsync(id);
            if (update != null)
            {
                _context.tblUpdate.Remove(update);
            }

            await _context.SaveChangesAsync();
            _alert.AddSuccessToastMessage("Update deleted.");
            return RedirectToAction(nameof(List));
        }
        private bool UpdateExists(int id)
        {
            return _context.tblUpdate.Any(e => e.id == id);
        }
    }
}
