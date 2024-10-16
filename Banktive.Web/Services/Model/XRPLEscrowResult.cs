namespace Banktive.Web.Services.Model
{
    public class XRPLCreateEscrowResult
    {
        public bool Successful { get; set; }
        public decimal? AmountDelivered { get; set; }
        public decimal? AmountSent { get; set; }
        public decimal? OriginalAmount { get; set; }
        public decimal? FeeAmount { get; set; }
        public uint? Sequence { get; set; }
        public string Hash { get; set; }
    }
}
