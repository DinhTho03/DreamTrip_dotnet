using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace brandportal_dotnet.Contracts.Faq.Faq
{
    public record PageFaqDetailDto
    {
        public string Id { get; set; }
        public string? Title { get; set; }
        public string? FaqContent { get; set; }
        public string? GroupTitle { get; set; }
        public uint? Position { get; set; }
        public bool? IsActived { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
