using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SL_Bullion.DAL;
using SL_Bullion.Models;
using SL_Bullion.Constant;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Security.Policy;
using System;

namespace SL_Bullion.Controllers
{
    public class ContactController : Controller
    {
        private readonly BullionDbContext _context;
        private readonly IToastNotification _alert;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationConstant _constatnt;

        public ContactController(BullionDbContext context, IToastNotification alert, IWebHostEnvironment webHostEnvironment, ApplicationConstant constatnt)
        {
            _context = context;
            _alert = alert;
            _webHostEnvironment = webHostEnvironment;
            _constatnt = constatnt;
        }

        // GET: Contact
        public async Task<IActionResult> List()
        {
            return View();
        }
        public JsonResult getContactDetails()
        {
            var result = _context.tblContact.Where(s => s.clientId == HttpContext.Session.GetInt32("clientId")).ToList();
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("id,clientId,marqueeTop,marqueeBottom,number1,number2,number3,number4,number5,number6,number7,whatsAppNo,address1,address2,address3,email1,email2,isBuy,isSell,isHigh,isLow,bannerWeb,bannerApp,modifiedDate,bannerWebImage,bannerAppImage")] Contact contact)
        {
            if (id != contact.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string? fileUrl = null;
                    contact.clientId = HttpContext.Session.GetInt32("clientId").GetValueOrDefault();
                    fileUrl = saveFile(contact.bannerWebImage, contact.clientId,"web");
                    contact.bannerWeb = fileUrl;
                    fileUrl = saveFile(contact.bannerAppImage, contact.clientId,"app");
                    contact.bannerApp = fileUrl;
                    var existingData = _context.tblContact.Where(e=> e.id == contact.id).SingleOrDefault();
                    if (existingData != null)
                    {
                        contact.isRate = existingData.isRate;
                        contact.goldDifferance = existingData.goldDifferance;
                        contact.silverDifferance = existingData.silverDifferance;
                        if (contact.bannerWebImage == null)
                        {
                            contact.bannerWeb = existingData.bannerWeb;
                        }
                        if (contact.bannerAppImage == null)
                        {
                            contact.bannerApp = existingData.bannerApp;
                        }
                        _context.Entry(existingData).State = EntityState.Detached;
                    }
                    
                    if (!clientExists(contact.clientId))
                    {
                        _context.Add(contact);
                    }
                    else
                    {
                        _context.Update(contact);
                    }
                    await _context.SaveChangesAsync();
                    _constatnt.pushContactDetails(contact.clientId);
                    _alert.AddSuccessToastMessage("Updated contact details.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.id))
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
            return View(contact);
        }

        private string saveFile(IFormFile? file,int clientId,string type)
        {
            string returnData = null;
            if (file != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images\\banner\\"+ clientId+"\\"+type);
                string fileName = file.FileName;
                returnData = Path.Combine($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + "/images/banner/" + clientId+"/"+ type +"/"+ fileName);
                string filePath = Path.Combine(uploadFolder, fileName);
                if (Directory.Exists(uploadFolder))
                {
                    Directory.Delete(uploadFolder, true);
                }
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string type)
        {
            try
            {
                string relativePath = "", filePath="";
                var contact = await _context.tblContact.FindAsync(id);
                if (type == "web" && contact != null && !string.IsNullOrEmpty(contact.bannerWeb))
                {
                    var uri = new Uri(contact.bannerWeb.Replace('\\', '/'));
                    relativePath = uri.AbsolutePath.TrimStart('/');
                    filePath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);
                    contact.bannerWeb = null;
                }
                else if (type == "app" && contact != null && !string.IsNullOrEmpty(contact.bannerApp))
                {
                    var uri = new Uri(contact.bannerApp.Replace('\\', '/'));
                    relativePath = uri.AbsolutePath.TrimStart('/');
                    filePath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);
                    contact.bannerApp = null;
                }
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                    _constatnt.pushContactDetails(contact.clientId);
                    _alert.AddSuccessToastMessage("Banner deleted.");
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            return RedirectToAction(nameof(List));
        }
        private bool clientExists(int clientId)
        {
            return _context.tblContact.Any(e => e.clientId == clientId);
        }

        private bool ContactExists(int id)
        {
            return _context.tblContact.Any(e => e.id == id);
        }
        
    }
}
