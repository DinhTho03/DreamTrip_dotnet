namespace brandportal_dotnet.Contracts.Client.Payment;

public class OrderInfo
{
    public string OrderId { get; set; }
    public int Amount { get; set; }
    public string Status { get; set; }
    public string OrderDesc { get; set; }
    public DateTime CreatedDate { get; set; }
    
}