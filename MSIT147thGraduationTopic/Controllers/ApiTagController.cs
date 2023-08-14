using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class ApiTagController : Controller
    {
        private readonly GraduationTopicContext _context;

        public ApiTagController(GraduationTopicContext context)
        {
            _context = context;
        }
        public IActionResult AutoCompleteOptions()
        {
            List<string> options = _context.Tags.Select(t => t.TagName).ToList();

            return Json(options);
        }
        public IActionResult CurrentTags(int id)
        {
            IEnumerable<SpecTagWithTagName> tags = _context.SpecTagWithTagNames.Where(st => st.SpecId == id);

            return Json(tags);
        }
    }
}
