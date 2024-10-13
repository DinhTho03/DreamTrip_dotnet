using brandportal_dotnet.Data.Utils;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("loyRewardRedeem")]
public class LoyRewardRedeem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    /// <summary>
    /// loy program
    /// </summary>
    public string? ProgramId { get; set; }

    /// <summary>
    /// id chương trình
    /// </summary>
    public string? RewardProgramId { get; set; }

    /// <summary>
    /// loại quà
    /// </summary>
    public string? RewardCode { get; set; }

    /// <summary>
    /// id quà
    /// </summary>
    public string? RewardObjectId { get; set; }

    /// <summary>
    /// user app
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// outlet master id
    /// </summary>
    public string? OutletId { get; set; }

    public string? CustomerCode { get; set; }

    public string? ShipToCode { get; set; }

    /// <summary>
    /// số lần đổi
    /// </summary>
    public int? Times { get; set; }

    /// <summary>
    /// điểm cần đổi
    /// </summary>
    public int? Point { get; set; }

    /// <summary>
    /// số lượng sản phẩm
    /// </summary>
    public int? Quota { get; set; }

    /// <summary>
    /// ngày hết hạn của quà
    /// </summary>
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Đã đổi quà chưa
    /// </summary>
    public bool? IsUsed { get; set; }

    /// <summary>
    /// ngày đổi quà
    /// </summary>
    public DateTime? DateUsed { get; set; }

    /// <summary>
    /// ngày đổi
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Số lượng quà tặng trong 1 gói quà tặng. Dùng để hiển thị số group by trong ưu đãi của tôi
    /// </summary>
    public int? QuantityUnit { get; set; }

    /// <summary>
    /// Trạng thái sử dụng
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Trạng thái sử dụng quà
    /// </summary>
    public string? DeliveryStatus { get; set; }

    /// <summary>
    /// Thời gian user đổi quà
    /// </summary>
    public DateTime? UseAt { get; set; }

    ///// <summary>
    ///// Thời gian brand đánh dấu đã giao hàng
    ///// </summary>
    //public DateTime? DeliveredAt { get; set; }

    ///// <summary>
    ///// Người cập nhật trạng thái đã giao hàng
    ///// </summary>
    //public int? DeliveredBy { get; set; }

    /// <summary>
    /// Thời gian User xác nhận đã nhận quà
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// 1: Hệ thống tự xác nhận nhận quà do quá thời hạn
    /// </summary>
    public bool? ForceConfirmed { get; set; }

    /// <summary>
    /// Số lượng quà tặng của đợt đổi
    /// </summary>
    public int? QuantityExchange { get; set; }

    /// <summary>
    /// Nguồn đổi ưu đãi
    /// </summary>
    public string? RedeemSource { get; set; }

}
