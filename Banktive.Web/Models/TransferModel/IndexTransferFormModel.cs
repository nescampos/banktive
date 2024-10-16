namespace Banktive.Web.Models.TransferModel
{
    public class IndexTransferFormModel
    {
        public int Page { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public long? WalletId { get; set; }
    }
}
