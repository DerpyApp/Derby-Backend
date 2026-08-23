namespace PadelBooking.BLL.Options
{
    public class PaymobOptions
    {
        public string BaseUrl { get; set; } = null!;
        public string ApiKey { get; set; } = null!;
        public string PublicKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
        public string HMAC { get; set; } = null!;
        public string WebhookRoute { get; set; } = null!;
        public PaymobEndpointsOptions Endpoints { get; set; } = new PaymobEndpointsOptions();
    }

    public class PaymobEndpointsOptions
    {
        public string Intention { get; set; } = null!;
        public string Refund { get; set; } = null!;
    }
}
