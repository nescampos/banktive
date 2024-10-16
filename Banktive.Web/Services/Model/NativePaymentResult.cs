namespace Banktive.Web.Services.Model
{
    public class NativePaymentResult
    {
        public bool Successful { get; set; }
        public decimal? AmountDelivered { get; set; }
        public decimal? AmountSent { get; set; }
        public decimal? OriginalAmount { get; set; }
        public decimal? FeeAmount { get; set; }
    }
}
