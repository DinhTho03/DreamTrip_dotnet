using brandportal_dotnet.Data.Utils;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace brandportal_dotnet.Data.Entities
{
    [BsonConllection("loyRewardProduct")]
    public class LoyRewardProduct
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        /// <summary>
        /// loy program
        /// </summary>
        public string? ProgramId { get; set; }

        /// <summary>
        /// id bảng loy_reward
        /// </summary>
        public string? RewardId { get; set; }

        /// <summary>
        /// id chương trình tiêu điểm
        /// </summary>
        public string? RewardProgramId { get; set; }

        /// <summary>
        /// id sản phẩm
        /// </summary>
        public string? ProductId { get; set; }

        /// <summary>
        /// id uom sản phẩm
        /// </summary>
        public string? ProductUomId { get; set; }

        public int? Quota { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
