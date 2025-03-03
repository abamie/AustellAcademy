using Razorpay.Api;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;


namespace AustellAcademyAdmissions.Service
{

public class RazorpayService
{
    private readonly string _keyId;
    private readonly string _keySecret;

    public RazorpayService(IConfiguration configuration)
    {
        _keyId = configuration["Razorpay:KeyId"];
        _keySecret = configuration["Razorpay:KeySecret"];
    }

    public Order CreateOrder(int amount, string currency = "INR")
    {
        var client = new RazorpayClient(_keyId, _keySecret);
        Dictionary<string, object> options = new Dictionary<string, object>
        {
            { "amount", amount * 100 },  // Convert to paise (1 INR = 100 paise)
            { "currency", currency },
            { "payment_capture", "1" }  // Auto capture payment
        };
        return client.Order.Create(options);
    }
}


}