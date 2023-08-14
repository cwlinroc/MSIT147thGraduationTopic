using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Interfaces;
using MSIT147thGraduationTopic.Models.Services;
using static MSIT147thGraduationTopic.Models.Infra.Utility.MailSetting;

namespace MSIT147thGraduationTopic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiSendMailController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly GraduationTopicContext _context;
        private readonly MemberService _service;
        private readonly IWebHostEnvironment _environment;

        public ApiSendMailController(GraduationTopicContext context
            , IWebHostEnvironment environment, IMailService mailService)
        {
            _context = context;
            _environment = environment;
            _service = new MemberService(context, environment);
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {  
            try
            {
                await _mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
