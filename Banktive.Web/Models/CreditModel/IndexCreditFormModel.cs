namespace Banktive.Web.Models.CreditModel
{
    public class IndexCreditFormModel
    {
        public int Page { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public long? WalletId { get; set; }
    }
}
