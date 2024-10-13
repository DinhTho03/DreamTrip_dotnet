using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.Contracts.Faq;
using brandportal_dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using brandportal_dotnet.IService;
using MongoDB.Driver.Linq;
using brandportal_dotnet.shared;
using brandportal_dotnet.Contracts.Faq.Faq;
using Volo.Abp.Domain.Entities;
using Volo.Abp;

namespace brandportal_dotnet.Controllers.Faqs
{
    [Route("[controller]")]
    [ApiController]
    public class FaqController : ControllerBase
    {
        private readonly IRepository<Faq> _faqService;
        private readonly IRepository<FaqGroup> _faqGroupService;
        public FaqController(IRepository<Faq> faqService, IRepository<FaqGroup> faqGroupService)
        {
            _faqService = faqService;
            _faqGroupService = faqGroupService;
        }

        /// <summary>
        /// Danh sách hỗ trợ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("~/api/pages/faq-content-all")]
        public async Task<PagedResultDto<PageFaqDto>> GetListFaqAsync(PagingRequestInput input)
        {
            var queryFaqAsync = await _faqService.GetAll();
            var queryFaqGroupAsync = await _faqGroupService.GetAll();
            var query = (from a in queryFaqAsync
                         where a.IsDeleted != true
                         join b in queryFaqGroupAsync on a.FaqGroup equals b._Id
                         select new PageFaqDto
                         {
                             Id = a._Id,
                             Title = a.FaqTitle,
                             GroupId = b._Id,
                             GroupTitle = b.FaqGroupTitle,
                             Position = a.FaqPosition,
                             IsActived = a.IsActived,
                             CreatedAt = a.CreatedAt
                         }).Where(input);

            var data = query
                .OrderByDescending(x => x.CreatedAt)
                .OrderBy(input)
                .Page(input).ToList();
            var total = query.Count();

            return new PagedResultDto<PageFaqDto>(total, data);
        }

        [HttpGet("~/api/pages/faq/{id}")]
        public async Task<PageFaqDetailDto> GetFaqDetailAsync(string id)
        {
            var queryFaqAsync = await _faqService.GetAll();
            var queryFaqGroupAsync = await _faqGroupService.GetAll();
            var query = from faq in queryFaqAsync
                        where faq._Id == id
                        join faqGroup in queryFaqGroupAsync on faq.FaqGroup equals faqGroup._Id into parentGroups
                        from parentGroup in parentGroups.DefaultIfEmpty()
                        select new PageFaqDetailDto
                        {
                            Id = faq._Id,
                            Title = faq.FaqTitle,
                            GroupTitle = parentGroup != null ? parentGroup.FaqGroupTitle : null,
                            Position = faq.FaqPosition,
                            IsActived = faq.IsActived,
                            CreatedAt = faq.CreatedAt,
                            FaqContent = faq.FaqContent
                        };

            var item = query.FirstOrDefault()
                       ?? throw new EntityNotFoundException(typeof(Faq), id);
            return item;
        }

        /// <summary>
        /// Danh sách hỗ trợ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("~/api/pages/faq")]
        public async Task<IActionResult> AddFaqAsync(PageFaqCreateDto input)
        {
            var now = DateTime.UtcNow;
            var faqGroupId = "";

            var errorMessages = new string[]
            {
            "Tên nội dung đã tồn tại",
            "Vị trí này đã bị trùng"
            };
            var parentFaqConditions = new Dictionary<string, object>
            {
                { "FaqGroupTitle", input.GroupTitle }
            };

            var parentFaqGroup = await _faqGroupService.FindByProperties(parentFaqConditions);

            if (parentFaqGroup != null)
            {
                faqGroupId = parentFaqGroup._Id;
            }

            var positionExistConditions = new Dictionary<string, object>
            {
                { "FaqPosition", input.Position },
                { "IsDeleted", false },
                { "FaqGroup", faqGroupId }
            };

            var existed = await _faqService.FindByProperties(positionExistConditions);
            var faqTitleTitleconditions = new Dictionary<string, object>
            {
                { "FaqTitle", input.Title },
                { "IsDeleted", false },
                { "FaqGroup", faqGroupId },
            };
            var existedFaqTitle = await _faqService.FindByProperties(faqTitleTitleconditions);

            if (existed != null && existedFaqTitle != null)
            {
                throw new UserFriendlyException(string.Join(", ", errorMessages));
            }

            if (existedFaqTitle != null)
            {
                return BadRequest(new { message = errorMessages[0] });
            }
 
            if (existed != null)
            {
                return BadRequest(new { message = errorMessages[1] });
            }


            var faq = new Faq
            {

                FaqTitle = input.Title,
                FaqContent = input.FaqContent,
                FaqType = "faq",
                FaqPosition = input.Position,
                FaqGroup = faqGroupId,
                IsActived = input.IsActived,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = 1,
                UpdatedBy = 1,
                IsDeleted = false,
            };

            await _faqService.Insert(faq);
            var result = new PageFaqDetailDto
            {
                Id = faq._Id
            };
            return Ok(result);
        }

        /// <summary>
        /// Danh sách hỗ trợ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("~/api/pages/faq/{id}")]
        public async Task<IActionResult> UpdateFaqGroupsAsync(string id, PageFaqCreateDto input)
        {
            var now = DateTime.UtcNow;
            var faq = await _faqService.GetById(id)
                 ?? throw new EntityNotFoundException(typeof(Faq), id);
            var faqGroupTitleconditions = new Dictionary<string, object>
            {
                { "FaqPosition", input.Position },
                { "IsDeleted", false },
                { "FaqGroup", faq.FaqGroup },
            };
            var existed = await _faqService.FindByProperties(faqGroupTitleconditions);
            if (existed != null && existed._Id != id)
            {
                return BadRequest(new { message = "Vị trí này đã bị trùng" });
            }
            string faqGroupId = "";
            if (!string.IsNullOrEmpty(input.GroupTitle))
            {
                var parentFaqGroupConditions = new Dictionary<string, object>
                {
                    { "FaqGroupTitle", input.GroupTitle }
                };
                var parentFaqGroup = await _faqGroupService.FindByProperties(parentFaqGroupConditions);
                faqGroupId = parentFaqGroup._Id;
            }
            faq.FaqContent = input.FaqContent;
            faq.FaqTitle = input.Title;
            faq.FaqGroup = faqGroupId;
            faq.FaqPosition = input.Position;
            faq.IsActived = input.IsActived;

            await _faqService.Update(id, faq);
            var result = new PageFaqDetailDto
            {
                Id = faq._Id
            };
            return Ok(result);
        }


        /// <summary>
        /// Danh sách hỗ trợ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("~/api/pages/faq/{id}")]
        public async Task<IActionResult> DeleteFaqTitleAsync(string id)
        {
            var faq = await _faqService.GetById(id);
            if (faq == null)
            {
                return BadRequest(new { message = "Nội dung hỗ trợ này không tồn tại" });
            }
            else
            {
                faq.IsDeleted = true;
                await _faqService.Update(id, faq);
                return Ok();
            }
        }

        /// <summary>
        /// Danh sách hỗ trợ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("~/api/pages/faq/{id}/status")]
        public async Task<IActionResult> UpdateStatusFaqAsync(string id, [FromBody] bool isActive)
        {
            var faq = await _faqService.GetById(id);
            if (faq == null)
            {
                return BadRequest(new { message = "Nội dung hỗ trợ này không tồn tại" });
            }

            faq.IsActived = isActive;
            await _faqService.Update(id, faq);
            return Ok();
        }
    }
}
