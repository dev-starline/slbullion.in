﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SL_Bullion.Constant;
using SL_Bullion.DAL;
using SL_Bullion.Models;
using Swashbuckle.Swagger;
using System.Text.Json.Nodes;

namespace SL_Bullion.Controllers
{
    public class CoinController : Controller
    {
        private readonly BullionDbContext _context;
        private readonly IToastNotification _alert;
        private readonly ApplicationConstant _constatnt;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CoinController(BullionDbContext context, IToastNotification alert, ApplicationConstant constatnt, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _alert = alert;
            _constatnt = constatnt;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> List()
        {
            return View(await _context.tblCoin.Where(s => s.clientId == HttpContext.Session.GetInt32("clientId")).OrderBy(s => s.index).ToListAsync());
        }
        public JsonResult getBankCalculation()
        {
            var bank = _context.tblCoinBank.Where(s => s.clientId == HttpContext.Session.GetInt32("clientId")).ToList();
            var contact = _context.tblContact.Where(s => s.clientId == HttpContext.Session.GetInt32("clientId")).Select(c => new
            {
                isRate = c.isCoinRate,
                isRateType= _context.tblCoin.Where(c => c.clientId == HttpContext.Session.GetInt32("clientId")).Select(c => c.rateType).FirstOrDefault()
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
        public async Task<IActionResult> Create([Bind("id,clientId,name,source,isView,rateType,buyPremium,sellPremium,division,multiply,gst,createDate,modifiedDate,changePremiumDate")] Coin symbol)
        {
            if (ModelState.IsValid)
            {
                symbol.clientId = HttpContext.Session.GetInt32("clientId").GetValueOrDefault();
                var data = _context.Add(symbol);
                await _context.SaveChangesAsync();
                _constatnt.setCoinSymbolRedis();
                var isSuccess = data.Entity.id;
                if (isSuccess > 0)
                {
                    _alert.AddSuccessToastMessage("Symbol created.");
                }
                return RedirectToAction(nameof(List));
            }
            return View(symbol);
        }

        [HttpPost]
        public async Task<IActionResult> updatePremium([FromBody] JsonObject obj)
        {
            var symbol = await _context.tblCoin.FindAsync(Convert.ToInt32(obj["id"].ToString()));
            if (symbol == null)
            {
                return NotFound();
            }
            symbol.isView = ((bool)obj["isView"]);
            symbol.isStock = ((bool)obj["isStock"]);
            symbol.name = obj["name"].ToString();
            symbol.buyPremium = obj["buyPremium"].ToString();
            symbol.sellPremium = obj["sellPremium"].ToString();
            symbol.division =Convert.ToDouble(obj["division"].ToString());
            symbol.multiply = Convert.ToDouble(obj["multiply"].ToString());
            if (TryValidateModel(symbol))
            {
                try
                {
                    var data = _context.Update(symbol);
                    await _context.SaveChangesAsync();
                    var isSuccess = data.Entity.id;
                    if (isSuccess > 0)
                    {
                        _constatnt.setCoinSymbolRedis();
                        _alert.AddSuccessToastMessage("Coin Symbol Edited.");
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
            else
            {
                var errorValue = ModelState[ModelState.Keys.FirstOrDefault()]?.Errors.FirstOrDefault()?.ErrorMessage;
                _alert.AddWarningToastMessage(errorValue?.ToString());
            }

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> saveAll([FromBody] JsonArray obj)
        {
            if (obj.Count > 0 && obj != null)
            {
                foreach (var item in obj)
                {
                    var symbol = await _context.tblCoin.FindAsync(Convert.ToInt32(item["id"].ToString()));
                    symbol.isView = ((bool)item["isView"]);
                    symbol.isStock = ((bool)item["isStock"]);
                    symbol.name = item["name"].ToString();
                    symbol.buyPremium = item["buyPremium"].ToString();
                    symbol.sellPremium = item["sellPremium"].ToString();
                    symbol.division = Convert.ToDouble(item["division"].ToString());
                    symbol.multiply = Convert.ToDouble(item["multiply"].ToString());

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
                _constatnt.setCoinSymbolRedis();
                _alert.AddSuccessToastMessage("Success");
            }
            return Ok();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var symbol = await _context.tblCoin.FindAsync(id);
            if (symbol != null)
            {
                _context.tblCoin.Remove(symbol);
            }

            await _context.SaveChangesAsync();
            _constatnt.setCoinSymbolRedis();
            _alert.AddSuccessToastMessage("Coin Symbol deleted.");
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        public async Task<IActionResult> changeRateType([FromBody] JsonObject obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE tblCoin SET rateType = {obj["rateType"].ToString()} WHERE clientId = {HttpContext.Session.GetInt32("clientId")}");
                    _constatnt.setCoinSymbolRedis();
                    _alert.AddSuccessToastMessage("Success.");
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }

            return Ok(200);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> updateBank(int id, [Bind("id,clientId,premiumGold,premiumSilver,spotTypeGold,spotTypeSilver,interBankGold,interBankSilver,conversionGold,conversionSilver,customDutyGold,customDutySilver,marginGold,marginSilver,gstGold,gstSilver,divisionGold,divisionSilver,multiplyGold,multiplySilver,modifiedDate")] CoinBank bank)
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
                    _constatnt.setCoinBankRateRedis();
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
                    await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE tblCoin SET buyCommonPremium = {obj["goldBuyCommonPremium"]?.ToString()},sellCommonPremium = {obj["goldSellCommonPremium"]?.ToString()} WHERE clientId = {HttpContext.Session.GetInt32("clientId")} and source='gold'");
                    await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE tblCoin SET buyCommonPremium = {obj["silverBuyCommonPremium"]?.ToString()},sellCommonPremium = {obj["silverSellCommonPremium"]?.ToString()} WHERE clientId = {HttpContext.Session.GetInt32("clientId")} and source='silver'");
                    _constatnt.setCoinSymbolRedis();
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
                    await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE tblContact SET isCoinRate = {bool.Parse(obj["isRate"].ToString())} WHERE clientId = {HttpContext.Session.GetInt32("clientId")}");
                    _constatnt.pushContactDetails((int)HttpContext.Session.GetInt32("clientId"));
                    _alert.AddSuccessToastMessage("Coin rate on/off edited.");
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
                        var symbol = await _context.tblCoin.FindAsync(Convert.ToInt32(item.ToString()));
                        symbol.index = index;
                        _context.Update(symbol);
                        await _context.SaveChangesAsync();
                        index++;
                    }
                    _constatnt.setCoinSymbolRedis();
                    _alert.AddSuccessToastMessage("Sequance edited.");
                }
                catch (Exception ex)
                {
                    _alert.AddErrorToastMessage("Invalid character.");
                }
            }

            return Ok(200);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coin = await _context.tblCoin.FindAsync(id);
            if (coin == null)
            {
                return NotFound();
            }
            var url = coin.url != null ? Url.Content("~/images/coin/" + coin.url) : Url.Content("~/img/noimage.png");

            ViewData["url"] = url;
            return PartialView("Action", coin);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Coin coin)
        {
            if (id != coin.id)
            {
                return NotFound();
            }
            try
            {
                string? fileUrl = null;
                var existingData = await _context.tblCoin.FindAsync(id);
                existingData.modifiedDate = DateTime.Now;
                fileUrl = saveFile(coin.image, coin.id);
                existingData.url = fileUrl;

                _context.Update(existingData);
                await _context.SaveChangesAsync();
                _constatnt.setCoinSymbolRedis();
                _alert.AddSuccessToastMessage("Coin image updated.");
            }
            catch (DbUpdateConcurrencyException)
            {
            }
            return RedirectToAction(nameof(List));
        }

        private string saveFile(IFormFile? file, int id)
        {
            string returnData = null;
            if (file != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images\\coin");
                string fileExtension = Path.GetExtension(file.FileName);
                returnData = id + fileExtension;
                string filePath = Path.Combine(uploadFolder, id + fileExtension);
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return returnData;
        }
        [HttpPost, ActionName("DeleteCoin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCoinConfirmed(int id)
        {
            try
            {
                string filePath = "";
                var coin = await _context.tblCoin.FindAsync(id);
                if (coin != null && !string.IsNullOrEmpty(coin.url))
                {
                    filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images\\coin", coin.url);
                    coin.url = null;
                }
               
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    _context.Update(coin);
                    await _context.SaveChangesAsync();
                    _alert.AddSuccessToastMessage("coin image deleted.");
                }
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction(nameof(List));
        }
        private bool SymbolExists(int id)
        {
            return _context.tblCoin.Any(e => e.id == id);
        }
        private bool clientExists(int clientId)
        {
            return _context.tblCoinBank.Any(e => e.clientId == clientId);
        }
    }
}
