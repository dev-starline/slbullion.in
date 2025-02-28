using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SL_Bullion.Models
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int clientId { get; set; }
        public string? marqueeTop { get; set; } 
        public string? marqueeBottom { get; set; } 
        public string? number1 { get; set; }
        public string? number2 { get; set; }
        public string? number3 { get; set; } 
        public string? number4 { get; set; }
        public string? number5 { get; set; } 
        public string? number6 { get; set; }
        public string? number7 { get; set; } 

        public string? whatsAppNo { get; set; } 
        public string? address1 { get; set; }
        public string? address2 { get; set; }
        public string? address3 { get; set; }
        public string? email1 { get; set; } 
        public string? email2 { get; set; } 
        public bool isBuy { get; set; } = true;
        public bool isSell { get; set; } = true;
        public bool isHigh { get; set; } = true;
        public bool isLow { get; set; } = true;
        public bool isRate { get; set; } = true;
        public bool isCoinRate { get; set; } = true;
        public string? bannerWeb { get; set; } 
        public string? bannerApp { get; set; }
        [NotMapped]
        public IFormFile? bannerWebImage { get; set; }
        [NotMapped]
        public IFormFile? bannerAppImage { get; set; }
        public int goldDifferance { get; set; } = 0;
        public int silverDifferance { get; set; }=0;
        public DateTime modifiedDate { get; set; } = DateTime.Now;
    }
}
