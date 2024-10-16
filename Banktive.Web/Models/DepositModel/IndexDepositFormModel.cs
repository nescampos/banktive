namespace Banktive.Web.Models.DepositModel
{
    public class IndexDepositFormModel
    {
        public int Page { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public long? WalletId { get; set; }
    }
}
