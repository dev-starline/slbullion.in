using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SL_Bullion.Constant;
using SL_Bullion.DAL;
using System.Text.Json.Nodes;

namespace SL_Bullion.Controllers
{
    public class SettingController : Controller
    {
        private readonly BullionDbContext _context;
        private readonly IToastNotification _alert;
        private readonly ApplicationConstant _constatnt;

        public SettingController(BullionDbContext context, IToastNotification alert, ApplicationConstant constatnt)
        {
            _context = context;
            _alert = alert;
            _constatnt = constatnt;
        }
        public async Task<IActionResult> List()
        {
            return View(await _context.tblReferanceSymbol.Where(s => s.clientId == HttpContext.Session.GetInt32("clientId")).ToListAsync());
        }
        public JsonResult getRateDifferance()
        {
            var contact = _context.tblContact.Where(s => s.clientId == HttpContext.Session.GetInt32("clientId")).Select(c => new
            {
                goldDifferance = c.goldDifferance,
                silverDifferance = c.silverDifferance
            }).ToList();
            
            return Json(contact);
        }

        [HttpPost]
        public async Task<IActionResult> updateReferance([FromBody] JsonObject obj)
        {
            var referance = await _context.tblReferanceSymbol.FindAsync((int)obj?["id"]);
            if (referance == null)
            {
                return NotFound();
            }
            referance.isView = (bool)obj["isView"];
            referance.name = obj["name"].ToString();
            if (TryValidateModel(referance))
            {
                try
                {
                    var data = _context.Update(referance);
                    await _context.SaveChangesAsync();
                    var isSuccess = data.Entity.id;
                    _constatnt.pushReferanceSymbol(referance.clientId);
                    _alert.AddSuccessToastMessage("Referance updated.");
                }
                catch (DbUpdateConcurrencyException)
                {
                   
                }
            }
            else
            {
                var errorValue = ModelState[ModelState.Keys.FirstOrDefault()]?.Errors.FirstOrDefault()?.ErrorMessage;
                _alert.AddWarningToastMessage(errorValue?.ToString());
            }

            return Ok(200);
        }
        [HttpPost]
        public async Task<IActionResult> updateRateDifferance([FromBody] JsonObject obj)
        {
            var contact = _context.tblContact.FirstOrDefault(s => s.clientId == HttpContext.Session.GetInt32("clientId"));
            if (contact == null)
            {
                return NotFound();
            }
            contact.goldDifferance = Convert.ToInt32(obj?["gold"].ToString());
            contact.silverDifferance = Convert.ToInt32(obj?["silver"].ToString());
            if (TryValidateModel(contact))
            {
                try
                {
                    var data = _context.Update(contact);
                    await _context.SaveChangesAsync();
                    var isSuccess = data.Entity.id;
                    _constatnt.pushRateDifferance();
                    _alert.AddSuccessToastMessage("Differance updated.");
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }
            else
            {
                var errorValue = ModelState[ModelState.Keys.FirstOrDefault()].Errors.FirstOrDefault()?.ErrorMessage;
                _alert.AddWarningToastMessage(errorValue?.ToString());
            }

            return Ok(200);
        }
        [HttpPost]
        public async Task<IActionResult> updatePassword([FromBody] JsonObject obj)
        {
            var master = await _context.tblMaster.FindAsync(HttpContext.Session.GetInt32("clientId"));
            if (master == null)
            {
                return NotFound();
            }
            if (master.password == obj["oldPassword"]?.ToString())
            {
                if (obj["newPassword"]?.ToString() == obj["confirmPassword"]?.ToString())
                {
                    master.password = obj["newPassword"]?.ToString();
                    var data = _context.Update(master);
                    await _context.SaveChangesAsync();
                    _alert.AddSuccessToastMessage("Password updated.");
                }
                else
                {
                    _alert.AddWarningToastMessage("Newpassword and confirm password not match.");
                }
            }
            else {
                _alert.AddErrorToastMessage("OldPassword not correct.");
            }
            return Ok(200);
        }
    }
}
