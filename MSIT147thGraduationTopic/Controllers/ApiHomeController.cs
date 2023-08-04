using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Services;

namespace MSIT147thGraduationTopic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiHomeController : ControllerBase
    {

        private readonly GraduationTopicContext _context;
        private readonly HomeServices _service;
        public ApiHomeController(GraduationTopicContext context)
        {
            _context = context;
            _service = new HomeServices(context);
        }




    }
}
