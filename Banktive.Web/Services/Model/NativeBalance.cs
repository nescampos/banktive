namespace Banktive.Web.Services.Model
{
    public class NativeBalance
    {
        public string? CurrencyCode { get; set; }
        public string? Issuer { get; set; }
        public string? Value { get; set; }
        public decimal? ValueAsNumber { get; set; }
        public decimal? ValueAsXrp { get; set; }
    }
}
