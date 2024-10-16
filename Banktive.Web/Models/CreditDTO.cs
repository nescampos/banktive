namespace Banktive.Web.Models
{
    public class CreditDTO
    {
        public Guid Id { get; set; }

        public long? WalletId { get; set; }

        public string? OriginAddress { get; set; }

        public decimal? Amount { get; set; }

        public string? DestinationAddress { get; set; }

        public string? Comments { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime DateToCash { get; set; }

        public string? AssetCode { get; set; }

        public string? Wallet { get; set; }

        public bool IsSend { get; set; }
    }
}
