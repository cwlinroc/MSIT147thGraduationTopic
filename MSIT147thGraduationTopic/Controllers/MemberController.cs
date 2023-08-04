using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using MSIT147thGraduationTopic.EFModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using MSIT147thGraduationTopic.Models.Services;
using MSIT147thGraduationTopic.Models.ViewModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class MemberController : Controller
    {
        private readonly GraduationTopicContext _context;
        private readonly MemberService _service;
        private readonly IWebHostEnvironment _environment;


        public MemberController(GraduationTopicContext context
                , IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _service = new MemberService(context, environment);
        }

        public IActionResult CreateMember()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }


        [Authorize(Roles = "會員")]
        public IActionResult MemberCenter()
        {
            return View();
        }


        [Authorize(Roles = "會員")]
        public IActionResult ShoppingHistory()
        {            
            return View();
        }

        //未登入時會自動移轉到此網址。
        public IActionResult NoLogin()
        {
            return View();
        }
        //未授權角色時會自動移轉到此網址。
        public IActionResult NoRole()
        {
            return View();
        }

        public IActionResult Cities()
        {
            var fileProvider = new PhysicalFileProvider(_environment.WebRootPath);
            var fileInfo = fileProvider.GetFileInfo("datas/CityCountyData.json");
            using (var stream = fileInfo.CreateReadStream())
            using (var reader = new StreamReader(stream))
            {
                JsonReader jsonReader = new JsonTextReader(reader);
                var json = (JArray)JToken.ReadFrom(jsonReader);
                var cities = json.Select(x => x["CityName"]);
                //return Json(cities.ToObject<string>());
                return Json(cities.Select(x => x.ToObject<string>()));
            }
        }

        public IActionResult Districts(string? city)
        {
            var fileProvider = new PhysicalFileProvider(_environment.WebRootPath);
            var fileInfo = fileProvider.GetFileInfo("datas/CityCountyData.json");
            using (var stream = fileInfo.CreateReadStream())
            using (var reader = new StreamReader(stream))
            {
                JsonReader jsonReader = new JsonTextReader(reader);
                var json = (JArray)JToken.ReadFrom(jsonReader);
                var areas = json
                    .FirstOrDefault(x => x["CityName"].ToObject<string>() == city)["AreaList"]
                    .Select(x => x["AreaName"]);
                return Json(areas.Select(x => x.ToObject<string>()));
            }
        }

    }
}
