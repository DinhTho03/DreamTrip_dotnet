using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using brandportal_dotnet.Data.Utils;

namespace brandportal_dotnet.Data.Entities
{
    [BsonConllection("pageCard")]
    public class PageCard
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }

        /// <summary>
        /// Hình ảnh đại diện
        /// </summary>
        public string PageCardImage { get; set; } = null!;

        /// <summary>
        /// Tiêu đề
        /// </summary>
        public string? PageCardTitle { get; set; }

        /// <summary>
        /// tag
        /// </summary>
        public string? PageCardTagline { get; set; }

        /// <summary>
        /// Vị trí xắp xếp
        /// </summary>
        public int PageCardPosition { get; set; }

        /// <summary>
        /// Trang đích
        /// </summary>
        public string? Action { get; set; }

        /// <summary>
        /// Params
        /// </summary>
        public string? ActionParams { get; set; }

        public bool? IsDeleted { get; set; }

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Người cập nhật
        /// </summary>
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