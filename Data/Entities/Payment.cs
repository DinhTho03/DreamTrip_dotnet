using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("payment")]
public class Payment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; } 
    
    public string? UserId { get; set; }
    
    public decimal? Amount { get; set; }
    
    public string? TransactionId { get; set; } = string.Empty;
    
    public string? BankCode { get; set; } = string.Empty;
    
    public string? OrderInfo { get; set; } = string.Empty;
    
    public DateTime? PaymentDate { get; set; }
    
    public string? CardType { get; set; } = string.Empty;
    
    public string? TransactionStatus { get; set; } = string.Empty;
    
    public string? SecureHash { get; set; } = string.Empty;
    public int? Point { get; set; } 
}