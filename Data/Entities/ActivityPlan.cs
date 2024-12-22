using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("activityPlan")]
public class ActivityPlan
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    
    public string? Name { get; set; }
    
    public string? SuggestPlanId { get; set; }
    
    public DateTime? CreateAt { get; set; }
    
    public DateTime? UpdateAt { get; set; }
    
    public string? CreateBy { get; set; }
    
    public string? UpdateBy { get; set; }
    
    public bool? IsActive { get; set; }
    
}