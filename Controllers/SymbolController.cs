using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SL_Bullion.Constant;
using SL_Bullion.DAL;
using SL_Bullion.Models;
using Swashbuckle.Swagger;

namespace SL_Bullion.Controllers
{
    public class SymbolController : Controller
    {
        private readonly BullionDbContext _context;
        private readonly IToastNotification _alert;
        private readonly ApplicationConstant _constatnt;

        public SymbolController(BullionDbContext context, IToastNotification alert, ApplicationConstant constatnt)
        {
            _context = context;
            _alert = alert;
            _constatnt = constatnt;
        }

        public async Task<IActionResult> List()
        {
            return View(await _context.tblSymbol.Where(s => s.clientId == HttpContext.Session.GetInt32("clientId")).OrderBy(s => s.index).ToListAsync());
        }
        public JsonResult getBankCalculation()
        {
            var bank=_context.tblBankRate.Where(s => s.clientId == HttpContext.Session.GetInt32("clientId")).ToList();
            var contact = _context.tblContact.Where(s => s.clientId == HttpContext.Session.GetInt32("clientId")).Select(c => new
            {
                isRate = c.isRate
            }).ToList();
            var obj = new
            {
                bank = bank,
                contact = contact
            };
            return Json(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,clientId,name,source,sourceType,isView,rateType,buyPremium,sellPremium,division,multiply,gst,createDate,modifiedDate,changePremiumDate")] Symbol symbol)
        {
            if (ModelState.IsValid)
            {
                symbol.clientId = HttpContext.Session.GetInt32("clientId").GetValueOrDefault();
                var data = _context.Add(symbol);
                await _context.SaveChangesAsync();
                _constatnt.setSymbolRedis();
                var isSuccess = data.Entity.id;
                if (isSuccess > 0)
                {
                    _alert.AddSuccessToastMessage("Symbol created.");
                }
                return RedirectToAction(nameof(List));
            }
            return View(symbol);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var symbol = await _context.tblSymbol.FindAsync(id);
            if (symbol == null)
            {
                return NotFound();
            }
            return PartialView("Action",symbol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,clientId,name,source,sourceType,isView,rateType,buyPremium,sellPremium,division,multiply,gst,digit,identifier,productType,description,createDate,modifiedDate,changePremiumDate")] Symbol symbol)
        {
            if (id != symbol.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingData = await _context.tblSymbol.FindAsync(id);
                    if (existingData == null)
                    {
                        return NotFound();
                    }
                    _context.Entry(existingData).State = EntityState.Detached;
                    symbol.clientId = HttpContext.Session.GetInt32("clientId").GetValueOrDefault();
                    symbol.buyCommonPremium = existingData.buyCommonPremium;
                    symbol.sellCommonPremium = existingData.sellCommonPremium;
                    symbol.index=existingData.index;
                    _context.Update(symbol);
                    await _context.SaveChangesAsync();
                    _constatnt.setSymbolRedis();
                    _alert.AddSuccessToastMessage("Symbol Edited.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SymbolExists(symbol.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(List));
            }
            return View(symbol);
        }

        [HttpPost]
        public async Task<IActionResult> updatePremium([FromBody] JsonObject obj)
        {
            var symbol = await _context.tblSymbol.FindAsync(Convert.ToInt32(obj["id"].ToString()));
            if (symbol == null)
            {
                return NotFound();
            }
            symbol.buyPremium = obj["buyPremium"]?.ToString()!="" ? obj["buyPremium"]?.ToString():"0"; 
            symbol.sellPremium = obj["sellPremium"]?.ToString() != "" ? obj["sellPremium"]?.ToString() : "0";
            if (TryValidateModel(symbol))
            {
                try
                {
                    var data = _context.Update(symbol);
                    await _context.SaveChangesAsync();
                    var isSuccess = data.Entity.id;
                    if (isSuccess > 0)
                    {
                        _constatnt.setSymbolRedis();
                        _alert.AddSuccessToastMessage("Symbol Edited.");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SymbolExists(symbol.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else {
                var errorValue = ModelState[ModelState.Keys.FirstOrDefault()]?.Errors.FirstOrDefault()?.ErrorMessage;
                _alert.AddWarningToastMessage(errorValue?.ToString());
            }

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> saveAll([FromBody] JsonArray obj)
        {
            if (obj.Count>0 && obj != null)
            {
                foreach (var item in obj)
                {
                    var symbol = await _context.tblSymbol.FindAsync(Convert.ToInt32(item["id"].ToString()));
                    symbol.buyPremium = item["buyPremium"]?.ToString() != "" ? item["buyPremium"]?.ToString() : "0";
                    symbol.sellPremium = item["sellPremium"]?.ToString() != "" ? item["sellPremium"]?.ToString() : "0";

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(symbol);
                            await _context.SaveChangesAsync();
                            
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!SymbolExists(symbol.id))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
                _constatnt.setSymbolRedis();
                _alert.AddSuccessToastMessage("Success");
            }
            return Ok();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var symbol = await _context.tblSymbol.FindAsync(id);
            if (symbol != null)
            {
                _context.tblSymbol.Remove(symbol);
            }

            await _context.SaveChangesAsync();
            _constatnt.setSymbolRedis();
            _alert.AddSuccessToastMessage("Symbol deleted.");
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        public async Task<IActionResult> changeRateType([FromBody] JsonObject obj)
        {          
            if (ModelState.IsValid)
            {               
                try
                {
                    await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE tblSymbol SET rateType = {obj["rateType"].ToString()} WHERE clientId = {HttpContext.Session.GetInt32("clientId")}");
                    _constatnt.setSymbolRedis();
                    _alert.AddSuccessToastMessage("Symbol edited.");
                }
                catch (DbUpdateConcurrencyException)
                {
                   
                }
            }

            return Ok(200);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> updateBank(int id, [Bind("id,clientId,premiumGold,premiumSilver,spotTypeGold,spotTypeSilver,interBankGold,interBankSilver,conversionGold,conversionSilver,customDutyGold,customDutySilver,marginGold,marginSilver,gstGold,gstSilver,divisionGold,divisionSilver,multiplyGold,multiplySilver,modifiedDate")] BankRate bank)
        {
            if (id != bank.id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    bank.clientId = HttpContext.Session.GetInt32("clientId").GetValueOrDefault();
                    if (!clientExists(bank.clientId))
                    {
                        _context.Add(bank);                        
                    }
                    else
                    {
                        _context.Update(bank);
                    }
                    await _context.SaveChangesAsync();
                    _constatnt.setBankRateRedis();
                    _alert.AddSuccessToastMessage("Bank calculation updated.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SymbolExists(bank.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(List));
            }
            return View(bank);
        }

        [HttpPost]
        public async Task<IActionResult> setCommonPremium([FromBody] JsonObject obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE tblSymbol SET buyCommonPremium = {obj["goldBuyCommonPremium"]?.ToString()},sellCommonPremium = {obj["goldSellCommonPremium"]?.ToString()} WHERE clientId = {HttpContext.Session.GetInt32("clientId")} and source='gold'");
                    await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE tblSymbol SET buyCommonPremium = {obj["silverBuyCommonPremium"]?.ToString()},sellCommonPremium = {obj["silverSellCommonPremium"]?.ToString()} WHERE clientId = {HttpContext.Session.GetInt32("clientId")} and source='silver'");
                    _constatnt.setSymbolRedis();
                    _alert.AddSuccessToastMessage("CommonPremium edited.");
                }
                catch (Exception ex)
                {
                    _alert.AddErrorToastMessage("Invalid character.");
                }
            }

            return Ok(200);
        }
        [HttpPost]
        public async Task<IActionResult> isRateUpdate([FromBody] JsonObject obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE tblContact SET isRate = {bool.Parse(obj["isRate"].ToString())} WHERE clientId = {HttpContext.Session.GetInt32("clientId")}");
                    _constatnt.pushContactDetails((int)HttpContext.Session.GetInt32("clientId"));
                    _alert.AddSuccessToastMessage("Symbol edited.");
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }

            return Ok(200);
        }
        [HttpPost]
        public async Task<IActionResult> updateSequance([FromBody] JsonArray obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int index = 1;
                    foreach (var item in obj)
                    {
                        var symbol = await _context.tblSymbol.FindAsync(Convert.ToInt32(item.ToString()));
                        symbol.index = index;
                        _context.Update(symbol);
                        await _context.SaveChangesAsync();
                        index++;
                    }
                    _constatnt.setSymbolRedis();
                    _alert.AddSuccessToastMessage("CommonPremium edited.");
                }
                catch (Exception ex)
                {
                    _alert.AddErrorToastMessage("Invalid character.");
                }
            }

            return Ok(200);
        }
        private bool SymbolExists(int id)
        {
            return _context.tblSymbol.Any(e => e.id == id);
        }
        private bool clientExists(int clientId)
        {
            return _context.tblBankRate.Any(e => e.clientId == clientId);
        }
    }
}
