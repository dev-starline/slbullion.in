using SL_Bullion.DAL;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace SL_Bullion.Constant
{
    public class ApplicationConstant
    {
        private readonly BullionDbContext _context;
        private readonly IConfiguration _config;
        private static readonly HttpClient client = new HttpClient();
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
        alertBody _alert = new alertBody();
        string adminNodeUrl = "", rateDiffNodeUrl=""; 

        public ApplicationConstant(BullionDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            adminNodeUrl = config.GetSection("adminNodeUrl").Value;
            rateDiffNodeUrl = config.GetSection("rateDiffNodeUrl").Value;
        }
        public class alertBody
        {
            public string? user { get; set; }
            public string? title { get; set; }
            public object? message { get; set; }
            public string? bit { get; set; }
        }
        private void setValueRedis(string key, string value)
        {
            IDatabase _db = redis.GetDatabase();
            _db.StringSet(key, value);
        }
        private async Task<string> getValueRedis(string key)
        {
            IDatabase _db = redis.GetDatabase();
            return await _db.StringGetAsync(key);
        }
        internal void setSymbolRedis()
        {
            var result = _context.tblSymbol
                    .Where(s =>s.isView==true).OrderBy(s => s.clientId).ThenBy(s => s.index)
                    .Select(s => new
                    {
                        id = s.id,
                        user = _context.tblMaster.Where(m => m.id == s.clientId).Select(m =>m.userName).FirstOrDefault(),
                        name = s.name,
                        source = s.source,
                        sourceType = s.sourceType,
                        rateType = s.rateType,
                        buyPremium = s.buyPremium,
                        sellPremium = s.sellPremium,
                        division = s.division,
                        multiply = s.multiply,
                        gst = s.gst,
                        buyCommonPremium = s.buyCommonPremium,
                        sellCommonPremium = s.sellCommonPremium,
                        digit=s.digit,
                        productType = s.productType,
                        description = s.description,
                        uniqueId=s.uniqueId,
                        identifier = s.identifier
                    })
                    .ToList();
            
            string jsonString=JsonSerializer.Serialize(result);
            setValueRedis("symbolDetails", jsonString);         
        }
        internal void setCoinSymbolRedis()
        {
            var result = _context.tblCoin
                    .Where(s => s.isView == true).OrderBy(s => s.clientId).ThenBy(s => s.index)
                    .Select(s => new
                    {
                        id = s.id,
                        user = _context.tblMaster.Where(m => m.id == s.clientId).Select(m => m.userName).FirstOrDefault(),
                        name = s.name,
                        source = s.source,
                        rateType = s.rateType,
                        buyPremium = s.buyPremium,
                        sellPremium = s.sellPremium,
                        division = s.division,
                        multiply = s.multiply,
                        buyCommonPremium = s.buyCommonPremium,
                        sellCommonPremium = s.sellCommonPremium,
                        url= s.url

                    })
                    .ToList();

            string jsonString = JsonSerializer.Serialize(result);
            setValueRedis("coinSymbolDetails", jsonString);
        }
        internal void setBankRateRedis()
        {
            var result = _context.tblBankRate
                    .Select(s => new
                    {
                        id = s.id,
                        user = _context.tblMaster.Where(m => m.id == s.clientId).Select(m => m.userName).FirstOrDefault(),
                        premiumGold = s.premiumGold,
                        premiumSilver = s.premiumSilver,
                        spotTypeGold = s.spotTypeGold,
                        spotTypeSilver = s.spotTypeSilver,
                        interBankGold = s.interBankGold,
                        interBankSilver = s.interBankSilver,
                        conversionGold = s.conversionGold,
                        conversionSilver = s.conversionSilver,
                        customDutyGold = s.customDutyGold,
                        customDutySilver = s.customDutySilver,
                        marginGold = s.marginGold,
                        marginSilver = s.marginSilver,
                        gstGold = s.gstGold,
                        gstSilver = s.gstSilver,
                        divisionGold = s.divisionGold,
                        divisionSilver = s.divisionSilver,
                        multiplyGold = s.multiplyGold,
                        multiplySilver = s.multiplySilver
                    })
                    .ToList();

            string jsonString = JsonSerializer.Serialize(result);
            setValueRedis("bankRateDetails", jsonString);
        }
        internal void setCoinBankRateRedis()
        {
            var result = _context.tblCoinBank
                    .Select(s => new
                    {
                        id = s.id,
                        user = _context.tblMaster.Where(m => m.id == s.clientId).Select(m => m.userName).FirstOrDefault(),
                        premiumGold = s.premiumGold,
                        premiumSilver = s.premiumSilver,
                        spotTypeGold = s.spotTypeGold,
                        spotTypeSilver = s.spotTypeSilver,
                        interBankGold = s.interBankGold,
                        interBankSilver = s.interBankSilver,
                        conversionGold = s.conversionGold,
                        conversionSilver = s.conversionSilver,
                        customDutyGold = s.customDutyGold,
                        customDutySilver = s.customDutySilver,
                        marginGold = s.marginGold,
                        marginSilver = s.marginSilver,
                        gstGold = s.gstGold,
                        gstSilver = s.gstSilver,
                        divisionGold = s.divisionGold,
                        divisionSilver = s.divisionSilver,
                        multiplyGold = s.multiplyGold,
                        multiplySilver = s.multiplySilver
                    })
                    .ToList();

            string jsonString = JsonSerializer.Serialize(result);
            setValueRedis("coinBankRateDetails", jsonString);
        }
        internal void setUserRedis()
        {
            string jsonString = "";
            var result = _context.tblMaster.Where(s => s.isActive == true)
                    .Select(s => new
                    {
                        user = s.userName
                    }).ToList();
            jsonString = JsonSerializer.Serialize(result);
            setValueRedis("userDetails", jsonString);

            var resultCoin = _context.tblMaster.Where(s => s.isActive == true && s.isCoin==true)
                    .Select(s => new
                    {
                        user = s.userName
                    }).ToList();
            jsonString = JsonSerializer.Serialize(resultCoin);
            setValueRedis("userCoinDetails", jsonString);
        }
        internal void pushRateDifferance()
        {
            var result = _context.tblContact
                    .Select(s => new
                    {
                        user = _context.tblMaster.Where(m => m.id == s.clientId).Select(m => m.userName).FirstOrDefault(),
                        fcmKey = _context.tblMaster.Where(m => m.id == s.clientId).Select(m => m.fcmKey).FirstOrDefault(),
                        premiumGold = s.goldDifferance,
                        premiumSilver = s.silverDifferance
                    })
                    .ToList();

            string jsonString = JsonSerializer.Serialize(result);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = client.PostAsync(rateDiffNodeUrl + "/rateDiffNotification", content);
        }
        internal void pushContactDetails(int clientId)
        {
            var result = _context.tblContact
                     .Select(s => new
                     {
                         activeUser = _context.tblMaster.Where(m => m.id == clientId).Select(m => m.userName).FirstOrDefault(),
                         user = _context.tblMaster.Where(m => m.id == s.clientId).Select(m => m.userName).FirstOrDefault(),
                         number1 = s.number1,
                         number2 = s.number2,
                         number3 = s.number3,
                         number4 = s.number4,
                         number5 = s.number5,
                         number6 = s.number6,
                         number7 = s.number7,
                         marqueeTop = s.marqueeTop,
                         whatsAppNo = s.whatsAppNo,
                         marqueeBottom = s.marqueeBottom,
                         address1 = s.address1,
                         address2 = s.address2,
                         address3 = s.address3,
                         email1 = s.email1,
                         email2 = s.email2,
                         bannerWeb = s.bannerWeb,
                         bannerApp = s.bannerApp,
                         isBuy = s.isBuy,
                         isSell = s.isSell,
                         isHigh = s.isHigh,
                         isLow = s.isLow,
                         isRate = s.isRate,
                         isCoin = s.isCoinRate
                     })
                     .ToList();
            string jsonString = JsonSerializer.Serialize(result);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = client.PostAsync(adminNodeUrl + "/contactDetails", content);
        }
        internal void pushReferanceSymbol(int clientId)
        {
            string activeUser = _context.tblMaster.Where(m => m.id == clientId).Select(m => m.userName).FirstOrDefault();
            var result = _context.tblReferanceSymbol
                     .Where(s => s.isMaster == true && s.isView == true)
                     .Select(s => new
                     {
                         user = _context.tblMaster.Where(m => m.id == s.clientId).Select(m => m.userName).FirstOrDefault(),
                         name = s.name,
                         source = s.source
                     })
                     .ToList();
            string jsonString = JsonSerializer.Serialize(result);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            content.Headers.Add("activeUser", activeUser);
            var response = client.PostAsync(adminNodeUrl+"/referanceDetails", content);
        }
        internal void pushAlert(int clientId,string title,string message)
        {
            var result = _context.tblMaster.Where(m => m.id == clientId).Select(m => new{activeUser = m.userName, key = m.fcmKey }).FirstOrDefault();
            _alert.user=result.activeUser;
            _alert.title=title;
            _alert.message=message;
            _alert.bit = "1";
            string jsonString = JsonSerializer.Serialize(_alert);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = client.PostAsync(adminNodeUrl + "/alertDetails", content);
            response = client.PostAsync(rateDiffNodeUrl + "/updateNotification", content);
        }
        
        internal async Task<string> getClientRateCode(int clientId)
        {
            string result = await getValueRedis(_config[$"clientRate:{clientId.ToString()}"]);
            return result;
        }
    }
}
