using brandportal_dotnet.Contracts.Client.FAQ;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using Microsoft.AspNetCore.Mvc;

namespace brandportal_dotnet.Controllers.Client.FAQ;

[Route("[controller]")]
[ApiController]
public class FAQUserController : ControllerBase
{
    private readonly IRepository<FaqGroup> _faqGroupService;
    private readonly IRepository<Faq> _faqService;

    public FAQUserController
    (
        IRepository<FaqGroup> faqGroupService,
        IRepository<Faq> faqService
    )
    {
        _faqGroupService = faqGroupService;
        _faqService = faqService;
    }

    [HttpGet("~/api/page-user/faq/content")]
    public async Task<FAQGroupResponseDto> GetFaqDataAsync()
    {
        var faqGroups = await _faqGroupService.GetAll();

        var faqs = await _faqService.GetAll();

        var faqGroupDtos = faqGroups
            .Select(group => new FAQGroupDto
            {
                _Id = group._Id,
                FaqGroupName = group.FaqGroupTitle,
                FaqGroupPosition = group.FaqGroupPosition,
                IsActived = group.IsActived,
                FAQ = faqs
                    .Where(f => f.FaqGroup == group._Id) 
                    .Select(faq => new FAQDto
                    {
                        Id = faq._Id,
                        FaqGroup = faq.FaqGroup,
                        FaqType = faq.FaqType,
                        FaqTitle = faq.FaqTitle,
                        FaqContent = faq.FaqContent,
                        FaqPosition = faq.FaqPosition,
                        IsActived = faq.IsActived
                    })
                    .OrderBy(faq => faq.FaqPosition) 
                    .ToArray()
            })
            .OrderBy(group => group.FaqGroupPosition) 
            .ToList();

        var response = new FAQGroupResponseDto
        {
            FaqGroup = faqGroupDtos.ToArray()
        };

        return response;
    }

}