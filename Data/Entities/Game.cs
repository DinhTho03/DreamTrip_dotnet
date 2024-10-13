using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using brandportal_dotnet.Data.Utils;

namespace brandportal_dotnet.Data.Entities
{
    [BsonConllection("game")]
    public class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        /// <summary>
        /// id chương trình tiêu điểm
        /// </summary>
        public string? RewardProgramId { get; set; }

        public string? CateId { get; set; }

        /// <summary>
        /// loại chương trình
        /// </summary>
        public string? CateType { get; set; }

        /// <summary>
        /// Tên chương trình
        /// </summary>
        public string GameName { get; set; } = null!;

        /// <summary>
        /// Mã chương trình
        /// </summary>
        public string? GameCode { get; set; }

        /// <summary>
        /// banner
        /// </summary>
        public string? Banner { get; set; }

        /// <summary>
        /// thể lệ chương trình
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// mô tả ngắn
        /// </summary>
        public string? Intro { get; set; }

        /// <summary>
        /// thời gian hiệu lực chương trình
        /// </summary>
        public string PeriodType { get; set; } = null!;

        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Tần suất chơi gảm
        /// </summary>
        public string Frequency { get; set; } = null!;

        /// <summary>
        /// Weekly: 0,1,2,3,4,5,6; Monthly: 1 .... n
        /// </summary>
        public string? FrequencyValue { get; set; }

        /// <summary>
        /// Loại nếu là hàng tháng
        /// </summary>
        public string? FrequencyMonthlyType { get; set; }

        /// <summary>
        /// nếu giá trị là ngày trong tháng: value 1,2,3 ... n, -1 là ngày cuối cùng
        /// </summary>
        public string? DayInMonthly { get; set; }

        /// <summary>
        /// nếu giá trị là ngày trong tuần 0,1,2,3, -1 nếu là tuần cuối
        /// </summary>
        public string? DayInWeek { get; set; }

        /// <summary>
        /// nếu giá trị là ngày trong tuần lặp lại vào thứ 0,1,2,3,4,5,6
        /// </summary>
        public string? DayInWeekRepeat { get; set; }

        /// <summary>
        /// thời gian thực hiện trong ngày
        /// </summary>
        public string PeriodInDateType { get; set; } = null!;

        /// <summary>
        /// từ giờ
        /// </summary>
        public string? PeriodInDateStart { get; set; }

        /// <summary>
        /// đến giờ
        /// </summary>
        public string? PeriodInDateEnd { get; set; }

        /// <summary>
        /// Cài đặt lượt quay : minus_point_directly - trừ điểm trực tiếp, redeem_accumulated_point - đổi điểm
        /// </summary>
        public string? ConfigTurn { get; set; }

        /// <summary>
        /// 1 : 100 thắng, 0 có thể không thắng
        /// </summary>
        public bool? WinType { get; set; }

        /// <summary>
        /// số lần thắng giải : -1 không giới hạn, lớn hơn 0 là giới hạng
        /// </summary>
        public short? WinQuotaLimit { get; set; }

        /// <summary>
        /// số lượng vòng quay, 8, 10 12
        /// </summary>
        public sbyte? NumbetWheel { get; set; }

        /// <summary>
        /// đối tượng tham gia:
        /// all tất cả.
        /// outlet : theo từng outlet
        /// group: Theo nhóm khách hàng
        /// </summary>
        public string? ObjectApply { get; set; }

        /// <summary>
        /// Đã update tab thông tin hiển thị chưa
        /// </summary>
        public bool? IsUpdateInfo { get; set; }

        /// <summary>
        /// đã update tab tỷ lệ thắng giải chưa
        /// </summary>
        public bool? IsUpdateWin { get; set; }

        /// <summary>
        /// Trạng thái game
        /// </summary>
        public bool? IsActived { get; set; }

        /// <summary>
        /// Trạng thái của game
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Đánh dấu xóa
        /// </summary>
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
        /// Áp dụng ngân sách tổng. Áp dụng cho game NPP. 1: Áp dụng; 0: Không áp dụng
        /// </summary>
        public bool? ApplyTotalBudget { get; set; }
    }
}
