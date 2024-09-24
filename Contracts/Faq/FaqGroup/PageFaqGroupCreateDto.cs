using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace brandportal_dotnet.Contracts.Faq.FaqGroup
{
    public record PageFaqGroupCreateDto
    {
        public string FaqGroupTitle { get; set; }
        public string? FaqGroupParentTitle { get; set; }
        public uint? Position { get; set; }
        public bool? IsActived { get; set; }
    }
}
