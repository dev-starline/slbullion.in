using Microsoft.AspNetCore.Mvc;
using SL_Bullion.Constant;
using SL_Bullion.Models;
using System.Text.Json;

namespace SL_Bullion.Controllers
{
    public class ClientRateController : Controller
    {
        private readonly ApplicationConstant _constatnt;
        public ClientRateController(ApplicationConstant constatnt)
        {
            _constatnt = constatnt;
        }
        public async Task<IActionResult> List()
        {
            int clientId = HttpContext.Session.GetInt32("clientId").GetValueOrDefault();
            var jsonResult = await _constatnt.getClientRateCode(clientId);
            List<ClientRate> clientRate = JsonSerializer.Deserialize<List<ClientRate>>(jsonResult);
            return View(clientRate);
        }
    }
}
