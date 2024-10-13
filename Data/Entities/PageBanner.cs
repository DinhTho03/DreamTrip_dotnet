using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using brandportal_dotnet.Data.Utils;

namespace brandportal_dotnet.Data.Entities
{
    [BsonConllection("pageBanner")]
    public class PageBanner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }

        public string? PageId { get; set; }

        /// <summary>
        /// page_type bảng page
        /// </summary>
        public string? PageType { get; set; }

        public string? PageImg { get; set; }

        public int? PageOrder { get; set; }

        public string? PageTagline { get; set; }

        public string? PageTitle { get; set; }

        public string? ActionName { get; set; }

        /// <summary>
        /// đích đến
        /// </summary>
        public string? Action { get; set; }

        /// <summary>
        /// đích đến chi tiết
        /// </summary>
        public string? ActionParams { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        /// <summary>
        /// Hiển thị cho tất cả outlet
        /// </summary>
        public int? AllowAllOutlet { get; set; }

        // SPVBRAAI-2563: Bổ sung trạng thái và thời gian hiệu lực banner
        /// <summary>
        /// Trạng thái hiệu lực
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Thời gian bắt đầu hiệu lực
        /// </summary>
        public DateTime? StartEffectiveDate { get; set; }

        /// <summary>
        /// Thời gian kết thúc hiệu lực
        /// </summary>
        public DateTime? EndEffectiveDate { get; set; }
    }
}