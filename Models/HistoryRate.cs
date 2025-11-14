using System.ComponentModel.DataAnnotations;

namespace SL_Bullion.Models
{
    public class HistoryRate
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int clientId { get; set; }
        public string? symbolName { get; set; }
        public double openRate { get; set; }
        public double closeRate { get; set; }
        public double highRate { get; set; }
        public double lowRate { get; set; }

        public DateTime createDate { get; set; } = DateTime.Now;
    }
}

