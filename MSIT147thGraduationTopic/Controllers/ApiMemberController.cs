using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class ApiMemberController : Controller
    {
        private readonly GraduationTopicContext _context;

        public ApiMemberController(GraduationTopicContext context)
        {
            _context = context;
        }

        
    }
}
