namespace brandportal_dotnet.Contracts.Client.Payment;

public class PaymentRequest
{
    public string OrderInfo { get; set; }
    public decimal Amount { get; set; }
}