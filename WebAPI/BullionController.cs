using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SL_Bullion.Constant;
using SL_Bullion.DAL;
using SL_Bullion.Models;
using Swashbuckle.Swagger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SL_Bullion.WebAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class bullionController : Controller
    {
        private readonly BullionDbContext _context;
        private readonly ApplicationConstant _constatnt;
        private readonly ResponseMessage _message;
        ResponseBody _response = new ResponseBody();
        private readonly IWebHostEnvironment _webHostEnvironment;
        public bullionController(BullionDbContext context, ApplicationConstant constatnt, ResponseMessage message, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _constatnt = constatnt;
            _message = message;
            _webHostEnvironment = webHostEnvironment;
        }
        public class ResponseBody {
            public int code { get; set; } = 200;
            public string? message { get; set; }
            public object? data { get; set; }
        }
        private int getClientId(string user)
        {
            var clientId = _context.tblMaster.Where(s => s.userName == user).Select(c => c.id).FirstOrDefault();
            return clientId;
        }
        /// <remarks>
        /// Todo:
        ///user:userName of project,type:android or ios
        /// </remarks>
        [HttpGet("versionAndroidIos")]
        public JsonResult version(string user,string type)
        {
            try
            {
                var data = _context.tblMaster.Where(s => s.userName == user).Select(c => new
                {
                    version = (type == "android") ? c.versionAndroid : c.versionIos
                }).ToList();
                if (data.Count > 0)
                {
                    _response.data = data;
                }
                else {
                    _response.code = 400;
                    _response.message = _message.C101;
                }
               
            }
            catch (Exception)
            {
                throw;
            }
            
            return Json(_response);
        }
        [HttpGet("bankDetails")]
        public JsonResult bank(string user)
        {
            try
            {
                int clientId = getClientId(user);
                var data = _context.tblBank.Where(b => b.clientId==clientId).ToList();
                if (data.Count > 0)
                {
                    _response.data = data;
                }
                else
                {
                    _response.code = 400;
                    _response.message = _message.C101;
                }

            }
            catch (Exception)
            {
                throw;
            }

            return Json(_response);
        }
        /// <remarks>
        /// Todo:
        /// dateFormat:dd/MM/yyyy
        /// </remarks>
        [HttpGet("updateDetails")]
        public JsonResult update(string user,string fromDate, string toDate)
        {
            try
            {
                int clientId = getClientId(user);
                DateTime fromDateValue;
                DateTime toDateValue;
                string dateFormat = "dd/MM/yyyy";
                if (!DateTime.TryParseExact(fromDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDateValue) ||
                !DateTime.TryParseExact(toDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out toDateValue))
                {
                    _response.code = 400;
                    _response.message = _message.C103;
                    return Json(_response);
                }
                toDateValue = toDateValue.Date.Add(new TimeSpan(23, 59, 59));
                var data = _context.tblUpdate.Where(b => b.clientId == clientId && b.modifiedDate >= fromDateValue && b.modifiedDate <= toDateValue).ToList();
                if (data.Count > 0)
                {
                    _response.data = data;
                }
                else
                {
                    _response.code = 400;
                    _response.message = _message.C104;
                }

            }
            catch (Exception)
            {
                throw;
            }

            return Json(_response);
        }
        /// <remarks>
        /// Sample request:
        ///
        ///     {"user": "","name": "","mobile": "","email":"","subject":"","message":""}
        ///
        /// </remarks>
        [HttpPost("feedbackDetails")]
        public JsonResult feedback([FromBody] JsonObject obj)
        {
            try
            {
                int clientId = getClientId(obj["user"].ToString());
                Feedback feedback = new Feedback();
                feedback.clientId = clientId;
                feedback.name = obj["name"].ToString();
                feedback.mobile = obj["mobile"].ToString();
                feedback.email = obj["email"].ToString();
                feedback.subject = obj["subject"].ToString();
                feedback.message = obj["message"].ToString();
                if (ModelState.IsValid)
                {
                    _context.Add(feedback);
                    _context.SaveChanges();
                    _response.message= _message.C102;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Json(_response);
        }
        /// <remarks>
        /// Sample request:
        ///
        ///     {"user": "","name": "","mobile": "","firmname":"","city":""}
        ///
        /// </remarks>
        [HttpPost("otrDetails")]
        public JsonResult otr([FromBody] JsonObject obj)
        {
            try
            {
                int clientId = getClientId(obj["user"].ToString());
                Otr otr = new Otr();
                otr.clientId = clientId;
                otr.name = obj["name"].ToString();
                otr.mobile = obj["mobile"].ToString();
                otr.firmname = obj["firmname"].ToString();
                otr.city = obj["city"].ToString();
                otr.ip = HttpContext.Connection.RemoteIpAddress.ToString();
                if (ModelState.IsValid)
                {
                    if (_context.tblOtr.Any(o =>o.clientId==clientId && o.mobile == obj["mobile"].ToString()))
                    {
                        var objOtr = _context.tblOtr.Where(o => o.clientId == clientId && o.mobile == otr.mobile).FirstOrDefault();
                        objOtr.name = otr.name;
                        objOtr.firmname = otr.firmname;
                        objOtr.city = otr.city;
                        objOtr.ip = otr.ip;
                        objOtr.modifiedDate = DateTime.Now;
                        _context.Update(objOtr);
                    }
                    else
                    {
                        _context.Add(otr);
                    }
                    _context.SaveChanges();
                    _response.message = _message.C105;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Json(_response);
        }
        [HttpPost("kycDetails")]
        public async Task<IActionResult> kyc([FromForm] Kyc kyc)
        {
            int clientId = getClientId(kyc.user);
            string uploads = Path.Combine(_webHostEnvironment.WebRootPath, "images\\kyc\\", clientId.ToString(), kyc.mobile);
            var zipFileName = Path.Combine(uploads, kyc.name + "_" + kyc.mobile + "_kyc.zip");
            string url = Path.Combine($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}", "images\\kyc\\", clientId.ToString(), kyc.mobile,kyc.name + "_" + kyc.mobile + "_kyc.zip");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            
            if (kyc.Files != null)
            {
                if (kyc.Files.Count > 0)
                {
                    using (var zipToCreate = new FileStream(zipFileName, FileMode.Create))
                    {
                        using (var archive = new ZipArchive(zipToCreate, ZipArchiveMode.Create))
                        {
                            foreach (var file in kyc.Files)
                            {
                                var entry = archive.CreateEntry(file.FileName);

                                using (var entryStream = entry.Open())
                                using (var fileStream = file.OpenReadStream())
                                {
                                    await fileStream.CopyToAsync(entryStream);
                                }
                            }
                        }
                    }
                }
            }
            kyc.url = url;
            kyc.clientId= clientId;

            if (ModelState.IsValid)
            {
                if (_context.tblKyc.Any(k => k.mobile == kyc.mobile))
                {
                    var existingData = await _context.tblKyc.FirstOrDefaultAsync(k => k.mobile == kyc.mobile);
                    existingData.url = kyc.url;
                    _context.Update(existingData);
                }
                else
                {
                    _context.Add(kyc);                  
                }
                await _context.SaveChangesAsync();
                _response.message = _message.C106;
            }
            return Ok(_response);
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     {"user": ""}
        ///
        /// </remarks>
        /// 
        [HttpGet("GetSlider")]
        public async Task<IActionResult> GetSlider(string user)
        {
            var response = new ResponseBody();

            try
            {
                int clientId = getClientId(user);

                string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

                var sliderList = await _context.tblSlider
                    .Where(s => s.ClientId == clientId)
                    .Select(s => new
                    {
                        s.SliderId,

                        SliderThumbnailUrl = string.IsNullOrEmpty(s.SliderThumbnailPath)
                            ? null
                            : Path.Combine(baseUrl, s.SliderThumbnailPath.Replace("\\", "/")),

                        SliderUrl = string.IsNullOrEmpty(s.SliderPath)
                            ? null
                            : Path.Combine(baseUrl, s.SliderPath.Replace("\\", "/"))
                    })
                    .ToListAsync();

                if (sliderList.Count > 0)
                {
                    response.code = 200;
                    response.message = "Data fetched successfully";
                    response.data = sliderList;
                    return Ok(response);
                }

                response.code = 404;
                response.message = _message.C101;
                response.data = Array.Empty<object>();
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.code = 500;
                response.message = $"An internal server error occurred while fetching slider data: {ex.Message}";
                response.data = Array.Empty<object>();
                return StatusCode(500, response);
            }
        }

    }

}
