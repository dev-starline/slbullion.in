using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SL_Bullion.Constant;
using SL_Bullion.DAL;
using System.Globalization;
using System.Reflection.Metadata;

namespace SL_Bullion.Controllers
{
    public class OtrController : Controller
    {
        private readonly BullionDbContext _context;
        private readonly IToastNotification _alert;
        public OtrController(BullionDbContext context, IToastNotification alert)
        {
            _context = context;
            _alert = alert;
        }
        public async Task<IActionResult> List()
        {
            return View(await _context.tblOtr.Where(o => o.clientId == HttpContext.Session.GetInt32("clientId") && o.modifiedDate.Date == DateTime.Today).OrderByDescending(o => o.modifiedDate).ToListAsync());
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
            var otr = await _context.tblOtr.Where(o => o.clientId == HttpContext.Session.GetInt32("clientId") && o.modifiedDate.Date >= fromDateValue.Date &&
                        o.modifiedDate.Date <= toDateValue.Date).OrderByDescending(o => o.modifiedDate).ToListAsync();

            return View("List", otr);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var otr = await _context.tblOtr.FindAsync(id);
            if (otr != null)
            {
                _context.tblOtr.Remove(otr);
            }

            await _context.SaveChangesAsync();
            _alert.AddSuccessToastMessage("OTR deleted.");
            return RedirectToAction(nameof(List));
        }
    }
}
