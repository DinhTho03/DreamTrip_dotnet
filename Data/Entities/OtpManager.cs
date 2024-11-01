using brandportal_dotnet.Data.Utils;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace brandportal_dotnet.Data.Entities;

[BsonConllection("otpManager")]
public class OtpManager
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    
    public string? Email { get; set; }

    public string Otptext { get; set; } = null!;

    public string? Otptype { get; set; }

    public DateTime Expiration { get; set; }

    public DateTime? Createddate { get; set; }
}