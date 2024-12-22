using brandportal_dotnet.Data.Utils;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace brandportal_dotnet.Data.Entities
{
    [BsonConllection("account")]
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
        public string Password { get; set; }
        public string? PasswordRT { get; set; }
        public string? RoleId { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? LoginDate { get; set; }
        /// <summary>
        /// Đánh dấu xóa
        /// </summary>
        public bool? IsDeleted { get; set; }
        /// <summary>
        /// Ngày xóa
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Người xóa
        /// </summary>
        public uint? DeletedBy { get; set; }
        public int? Point { get; set; }
    }
}
