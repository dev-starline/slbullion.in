using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SL_Bullion.Models
{
    public class Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
        public bool isActive { get; set; } = true;
        [Required]
        public string firmName { get; set; }
        public string clientName { get; set; }
        public string mobile { get; set; }
        public string city { get; set; }
        public string? domain { get; set; }
        public int symbol { get; set; } = 10;
        public int group { get; set; } = 4;
        public string versionAndroid { get; set; } = "1.0";
        public string versionIos { get; set; } = "1.0";
        public string? fcmKey { get; set; }
        public bool isCoin { get; set; }=false;
        public bool isJewellery { get; set; }= false;
        public bool isKyc { get; set; } = false;
        public bool isOtr { get; set; } = false;
        public bool isUpdate { get; set; } = false;
        public bool isBank { get; set; } = false;
        public bool isFeedback { get; set; } = false;
        public bool isClientRate { get; set; } = false;
        [NotMapped]
        public IFormFile? privacyPolicyFile { get; set; }
        public DateTime createDate { get; set; } = DateTime.Now;
        public DateTime modifiedDate { get; set; }
    }
}
