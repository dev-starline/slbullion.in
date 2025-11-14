using Microsoft.EntityFrameworkCore;
using SL_Bullion.Models;

namespace SL_Bullion.DAL
{
    public class BullionDbContext:DbContext
    {
        public BullionDbContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<MasterLogin> tblMasterLogin { get; set; }
        public virtual DbSet<Master> tblMaster { get; set; }
        public virtual DbSet<ReferanceSymbol> tblReferanceSymbol { get; set; }
        public virtual DbSet<Symbol> tblSymbol { get; set; }
        public virtual DbSet<BankRate> tblBankRate { get; set; }
        public virtual DbSet<Contact> tblContact { get; set; }
        public virtual DbSet<Update> tblUpdate { get; set; }
        public virtual DbSet<Bank> tblBank { get; set; }
        public virtual DbSet<Feedback> tblFeedback { get; set; }
        public virtual DbSet<Otr> tblOtr { get; set; }
        public virtual DbSet<Coin> tblCoin { get; set; }
        public virtual DbSet<CoinBank> tblCoinBank { get; set; }
        public virtual DbSet<Kyc> tblKyc { get; set; }
        public virtual DbSet<BankLogo> tblBankLogo { get; set; }

        public virtual DbSet<HistoryRate> tblHistoryRate { get; set; }
        public virtual DbSet<Slider> tblSlider { get; set; }
	}

}
