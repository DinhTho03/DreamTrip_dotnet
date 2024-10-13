using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("loyAccumulationProgram")]
public class LoyAccumulationProgram
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }

    /// <summary>
    /// id chương trình loyalty
    /// </summary>
    public string? ProgramId { get; set; }

    /// <summary>
    /// mã chtrinh tích điểm
    /// </summary>
    public string? AccumulationProgramCode { get; set; }

    /// <summary>
    /// tên chương trình tích điểm
    /// </summary>
    public string? AccumulationProgramName { get; set; }

    /// <summary>
    /// loại chương trình áp dụng
    /// </summary>
    public string? SourcePointKey { get; set; }

    /// <summary>
    /// giá trị theo chương trình áp dụng
    /// </summary>
    public string? ObjId { get; set; }

    /// <summary>
    /// thời gian hiệu lực chương trình
    /// </summary>
    public string? ValidityPeriodType { get; set; }

    /// <summary>
    /// có giá trị nếu validity_period_type = time_limit
    /// </summary>
    public DateTime? DateStart { get; set; }

    /// <summary>
    /// có giá trị nếu validity_period_type = time_limit
    /// </summary>
    public DateTime? DateEnd { get; set; }

    /// <summary>
    /// cộng điểm tích lũy theo loại
    /// </summary>
    public string? ApplyType { get; set; }

    /// <summary>
    /// số điểm phân hạng, dành cho apply type = all
    /// </summary>
    public int? AccumulationPoint { get; set; }

    /// <summary>
    /// số điểm có thể sử dụng, dành cho apply type = all
    /// </summary>
    public int? AvailablePoint { get; set; }

    /// <summary>
    /// số điểm phân hạng khi fail, dành cho apply type = all
    /// </summary>
    public int? FailAccumulationPoint { get; set; }

    /// <summary>
    /// số điểm có thể sử dụng khi fail, dành cho apply type = all
    /// </summary>
    public int? FailAvailablePoint { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Chỉ áp dụng đối với 1 số chương trình tích điểm thôi: Xác nhận đơn hàng
    /// </summary>
    public sbyte? AllowAllOutlet { get; set; }

    /// <summary>
    /// Loại cộng điểm. success: Kết quả đạt; all: Đạt và không đạt
    /// </summary>
    public string? AccumulationProgramResultType { get; set; }

    /// <summary>
    /// Bật cấu hình giới hạn ngân sách tích điểm
    /// </summary>
    public sbyte? EnableBudget { get; set; }

    /// <summary>
    /// Cấu hình kiểu json nha VD: {&quot;accumulation_point_outlet_quota&quot;: 999, &quot;available_point_outlet_quota&quot;: 999}
    /// </summary>
    public string? BudgetConfig { get; set; }

    /// <summary>
    /// Cấu hình mở rộng, dạng JSON
    /// </summary>
    public string? ExtraConfig { get; set; }
}