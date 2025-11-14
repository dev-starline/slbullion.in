using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SL_Bullion.DAL;
using SL_Bullion.Models;

namespace SL_Bullion.Controllers
{
    public class HistoryRateController : Controller
    {
        private readonly BullionDbContext _context;
        private readonly IToastNotification _alert;

        public HistoryRateController(BullionDbContext context, IToastNotification alert)
        {
            _context = context;
            _alert = alert;
        }

        public async Task<IActionResult> List()
        {

            return View(await _context.tblHistoryRate.Where(s => s.clientId == HttpContext.Session.GetInt32("clientId")).OrderByDescending(s => s.createDate).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HistoryRate rate)
        {
            int? clientId = HttpContext.Session.GetInt32("clientId"); 
            if (!ModelState.IsValid)
            {
                _alert.AddErrorToastMessage("Please enter valid details.");
                return RedirectToAction(nameof(List));
            }           
            rate.clientId = clientId.Value;            
            _context.Add(rate);
            await _context.SaveChangesAsync();
            _alert.AddSuccessToastMessage("History Rate added successfully.");
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _context.tblHistoryRate.FindAsync(id);

            if (record == null)
            {
                _alert.AddErrorToastMessage("HistoryRate not found.");
                return RedirectToAction(nameof(List));
            }

            _context.tblHistoryRate.Remove(record);
            await _context.SaveChangesAsync();

            _alert.AddSuccessToastMessage("HistoryRate Deleted Successfully.");
            return RedirectToAction(nameof(List));
        }
    }
}
