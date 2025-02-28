using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SL_Bullion.Models
{
    public class Symbol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int clientId { get; set; }
        public string uniqueId { get; set; } = Guid.NewGuid().ToString().Substring(0, 13);
        [Required]
        public string name { get; set; }
        [Required]
        public string source { get; set; }
        [Required]
        public string sourceType { get; set; } = "C_BA";
        [Required]
        public bool isView { get; set; } = true;
        public string rateType { get; set; } = "mcx";
        public int productType { get; set; } = 1;
        public string? description { get; set; }

        [RegularExpression("^--|-?[0-9]+$", ErrorMessage = "Invalid format. Only -- or digits (0-9) are allowed.")]
        public string buyPremium { get; set; } = "0";
        [RegularExpression("^--|-?[0-9]+$", ErrorMessage = "Invalid format. Only -- or digits (0-9) are allowed.")]
        public string sellPremium { get; set; } = "0";
        public double division { get; set; } = 1;
        public double multiply { get; set; }= 1;
        public double gst { get; set; } = 0;

        public double buyCommonPremium { get; set; } = 0;
        public double sellCommonPremium { get; set; } = 0;
        public int index { get; set; } = 0;
        public int digit { get; set; } = 0;
        public string identifier { get; set; } = "0";
        public DateTime createDate { get; set; }= DateTime.Now;
        public DateTime modifiedDate { get; set; }
        public DateTime changePremiumDate { get; set; }
    }
}
