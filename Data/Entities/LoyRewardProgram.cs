using brandportal_dotnet.Data.Utils;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("loyRewardProgram")]
public class LoyRewardProgram
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    /// <summary>
    /// program : chương trình tiêu điểm, gift_wheel : quà tặng vòng xoay
    /// </summary>
    public string? RewardProgramType { get; set; }

    /// <summary>
    /// id chương trình loyalty
    /// </summary>
    public string? ProgramId { get; set; }

    /// <summary>
    /// loại chương trình áp dụng
    /// </summary>
    public string? SourcePointKey { get; set; }

    /// <summary>
    /// tên chương trình
    /// </summary>
    public string RewardProgramName { get; set; } = null!;

    /// <summary>
    /// mô tả
    /// </summary>
    public string RewardProgramDescription { get; set; } = null!;

    /// <summary>
    /// mã chương trình
    /// </summary>
    public string? RewardProgramCode { get; set; }

    /// <summary>
    /// banner hoặc hình ảnh
    /// </summary>
    public string? Banner { get; set; }

    /// <summary>
    /// ngày bắt đầu
    /// </summary>
    public DateTime? DateStart { get; set; }

    /// <summary>
    /// ngày kết thúc
    /// </summary>
    public DateTime? DateEnd { get; set; }

    /// <summary>
    /// điểm tích lũy cần đổi, nếu source_point_key = game_wheel thì điểm này dành cho tất cả thành viên
    /// </summary>
    public int? Point { get; set; }

    /// <summary>
    /// id bảng loy_reward
    /// </summary>
    public string? RewardId { get; set; }

    /// <summary>
    /// Cài đặt lượt quay : minus_point_directly - trừ điểm trực tiếp, redeem_accumulated_point - đổi điểm
    /// </summary>
    public string ConfigTurn { get; set; } = null!;

    /// <summary>
    /// loại tính hạn sử dụng của quà : day - + ngày đổi quà với value tương ứng, fixed_date : đúng ngày hiện tại sẽ hết hạn
    /// </summary>
    public string? RewardExpiredType { get; set; }

    /// <summary>
    /// giá trị của reward_expired_type, nếu day là interger, nếu fixed_date là datetime
    /// </summary>
    public string? RewardExpiredValue { get; set; }

    /// <summary>
    /// ngày hết hạn đã tính theo loại sau khi cấu hình
    /// </summary>
    public DateTime? RewardExpiredDate { get; set; }

    /// <summary>
    /// số lượng mỗi lần đổi điểm
    /// </summary>
    public sbyte? QuotaChange { get; set; }

    /// <summary>
    /// giới hạn số lần đổi của chương trình
    /// </summary>
    public bool? IsLimitedExchangeAll { get; set; }

    /// <summary>
    /// Loại giới hạn. Chương trình cũ thì chỉ giới hạn toàn chương trình và không giới hạn. Loại mới có bổ sung thêm NPP
    /// </summary>
    public string? LimitedExchangeType { get; set; }

    /// <summary>
    /// số lượng giới hạn đổi của chương trình
    /// </summary>
    public int? QuotaLimitedExchangeAll { get; set; }

    /// <summary>
    /// giới hạn số lần đổi của outlet
    /// </summary>
    public bool? IsLimitedExchangeOutlet { get; set; }

    /// <summary>
    /// số lượng giới hạn đổi của outlet
    /// </summary>
    public string? QuotaLimitedExchangeOutlet { get; set; }

    /// <summary>
    /// áp dụng cho hạng thành viên : all tất cả, rank : theo hạng, map với table loy_reward_program_rank.
    /// </summary>
    public string? MembershipType { get; set; }

    public string? Value { get; set; }

    public string? Background { get; set; }

    /// <summary>
    /// trạng thái
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// hiển thị trên danh sách đổi điểm ?
    /// </summary>
    public bool? IsDisplay { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Áp dụng loyalty
    /// </summary>
    public bool? ApplyLoyalty { get; set; }

    /// <summary>
    /// Tặng lượt chơi game theo: Chương trình, Ngày, Tuần, Tháng
    /// </summary>
    public string? BonusPlaysType { get; set; }

    /// <summary>
    /// Số lượng lượt quay mỗi lần được tặng
    /// </summary>
    public uint? PlayTurnNumber { get; set; }
}
