using brandportal_dotnet.Contracts.Faq;
using brandportal_dotnet.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Linq;
using MongoDB.Driver.Linq;
using brandportal_dotnet.Models;
using brandportal_dotnet.shared;
using System.Linq.Expressions;
using System.Linq;
using brandportal_dotnet.IService;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using brandportal_dotnet.Contracts.Faq.FaqGroup;

namespace brandportal_dotnet.Controllers.Faqs
{
    [ApiController]
    [Route("[controller]")]
    public class FaqGroupController : ControllerBase
    {
        private readonly IRepository<FaqGroup> _faqGroupService;

        public FaqGroupController(IRepository<FaqGroup> faqGroupService)
        {
            _faqGroupService = faqGroupService;
        }

        [HttpGet("~/api/faq-group/paged")]
        public async Task<PagedResultDto<PageFaqGroupDto>> GetListFaqGroupsAsync(PagingRequestInput input)
        {
            var queryFaqGroupAsync = await _faqGroupService.GetAll();

            var query = (from a in queryFaqGroupAsync
                         where a.IsDeleted == false
                         join b in queryFaqGroupAsync on a.ParentId equals b._Id into parentGroups
                         from parentGroup in parentGroups.DefaultIfEmpty()
                         select new PageFaqGroupDto
                         {
                             Id = a._Id,
                             FaqGroupTitle = a.FaqGroupTitle,
                             FaqGroupParentTitle = parentGroup != null ? parentGroup.FaqGroupTitle : null,
                             Position = a.FaqGroupPosition,
                             IsActived = a.IsActived,
                             CreatedAt = a.CreatedAt
                         }).Where(input).OrderByDescending(x => x.CreatedAt);

            var data = query
                .OrderBy(input)
                .Page(input).ToList();
            var total = query.Count();

            return new PagedResultDto<PageFaqGroupDto>(total, data);
        }

        [HttpGet("~/api/faq-group/paged/{id}")]
        public async Task<PageFaqGroupDto> GetFaqGroupsDetailAsync(string id)
        {
            var queryFaqGroupAsync = await _faqGroupService.GetAll();

            var query = from a in queryFaqGroupAsync
                         where a._Id == id
                         join b in queryFaqGroupAsync on a.ParentId equals b._Id into parentGroups
                         from parentGroup in parentGroups.DefaultIfEmpty()
                         select new PageFaqGroupDto
                         {
                             Id = a._Id,
                             FaqGroupTitle = a.FaqGroupTitle,
                             FaqGroupParentTitle = parentGroup != null ? parentGroup.FaqGroupTitle : null,
                             Position = a.FaqGroupPosition,
                             IsActived = a.IsActived,
                             CreatedAt = a.CreatedAt
                         };

            var item = query.FirstOrDefault()
                       ?? throw new EntityNotFoundException(typeof(FaqGroup), id);
            return item;
        }

        [HttpPost("~/api/faq-group/paged")]
        public async Task<PageFaqGroupDto> AddFaqGroupAsync(PageFaqGroupCreateDto input)
        {
            var now = DateTime.UtcNow;
            var parentId = "";
            var parentFaqConditions = new Dictionary<string, object>
            {
                { "FaqGroupTitle", input.FaqGroupParentTitle }
            };

            var parentFaqGroup = await _faqGroupService.FindByProperties(parentFaqConditions);
            if (parentFaqGroup != null)
            {
                parentId = parentFaqGroup._Id;
            }

            var positionExistConditions = new Dictionary<string, object>
            {
                { "FaqGroupPosition", input.Position },
                { "IsDeleted", false },
                { "ParentId", parentId }
            };

            var existed = await _faqGroupService.FindByProperties(positionExistConditions);

            var FaqGroupTitleconditions = new Dictionary<string, object>
            {
                { "FaqGroupTitle", input.FaqGroupTitle },
                { "IsDeleted", false },
            };
            var existedFaqGroupTitle = await _faqGroupService.FindByProperties(FaqGroupTitleconditions);

            var errorMessages = new string[]
            {
                "Tên nhóm nội dung đã tồn tại",
                "Vị trí này đã bị trùng"
            };

            if (existed != null && existedFaqGroupTitle != null)
            {
                throw new UserFriendlyException(string.Join(", ", errorMessages));
            }

            if (existedFaqGroupTitle != null)
            {
                throw new UserFriendlyException(errorMessages[0]);
            }

            if (existed != null)
            {
                throw new UserFriendlyException(errorMessages[1]);
            }

            var faqGroup = new FaqGroup
            {
                FaqGroupTitle = input.FaqGroupTitle,
                FaqGroupPosition = input.Position,
                IsActived = input.IsActived,
                FaqGroupType = "basic",
                ParentId = parentId,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = 1,
                UpdatedBy = 1,
                IsDeleted = false,
            };

            await _faqGroupService.Insert(faqGroup);
            var result = new PageFaqGroupDto
            {
                Id = faqGroup._Id
            };
            return result;
        }

        [HttpPut("~/api/faq-group/paged/{id}")]
        public async Task<PageFaqGroupDto> UpdateFaqGroupAsync(string id, PageFaqGroupCreateDto input)
        {
            var now = DateTime.UtcNow;
            var faqGroup = await _faqGroupService.GetById(id)
                 ?? throw new EntityNotFoundException(typeof(FaqGroup), id);
            var positionExistConditions = new Dictionary<string, object>
            {
                { "FaqGroupPosition", input.Position },
                { "IsDeleted", false },
                { "ParentId", faqGroup.ParentId },
            };
            var existed = await _faqGroupService.FindByProperties(positionExistConditions);

            if (existed != null && existed._Id != id)
            {
                throw new UserFriendlyException("Vị trí này đã bị trùng");
            }

            string? parentId = "";
            if (!string.IsNullOrEmpty(input.FaqGroupParentTitle))
            {
                var FaqGroupTitleconditions = new Dictionary<string, object>
            {
                { "FaqGroupTitle", input.FaqGroupParentTitle },
                { "IsDeleted", false },
            };
                var parentFaqGroup = await _faqGroupService.FindByProperties(FaqGroupTitleconditions);
                parentId = parentFaqGroup?._Id;
            }

            faqGroup.FaqGroupTitle = input.FaqGroupTitle;
            faqGroup.ParentId = parentId;
            faqGroup.FaqGroupPosition = input.Position;
            faqGroup.IsActived = input.IsActived;

            await _faqGroupService.Update(id, faqGroup);
            var result = new PageFaqGroupDto
            {
                Id = faqGroup._Id
            };
            return result;
        }

        [HttpDelete("~/api/faq-group/paged/{id}")]
        public async Task DeleteFaqGroupTitleAsync(string id)
        {

            var faqGroup = await _faqGroupService.GetById(id);
            if (faqGroup == null)
            {
                throw new UserFriendlyException("Nhóm hỗ trợ này không tồn tại");
            }
            else
            {
                await _faqGroupService.Delete(id);
            }
        }


        [HttpGet("~/api/faq-group/paged/filter-faq-group-title")]
        public async Task<List<PageFaqGroupTitleDto>> GetListFaqGroupsTitleAsync()
        {
            var query = from a in await _faqGroupService.GetAll()
                         where a.IsDeleted == false
                         select new PageFaqGroupTitleDto
                         {
                             Id = a._Id,
                             FaqGroupTitle = a.FaqGroupTitle,
                         };

            return query.ToList();
        }

        [HttpPut("~/api/faq-group/paged/{id}/status")]
        public async Task UpdateStatusFaqGroupAsync(string id, [FromBody] bool isActive)
        {
            var faq = await _faqGroupService.GetById(id);
            if (faq == null)
            {
                throw new UserFriendlyException("Nội dung hỗ trợ này không tồn tại");
            }

            faq.IsActived = isActive;
            await _faqGroupService.Update(id, faq);
        }

    }
}
