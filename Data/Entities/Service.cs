using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace brandportal_dotnet.Data.Entities
{
    [BsonConllection("service")]
    public class Service
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public DateTime? created { get; set; } 
        public DateTime? updated { get; set;}
        public int qualityView { get; set; }
        public string cateId { get; set; }

    }
}
