using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Services;
using System.Security.Claims;
using MSIT147thGraduationTopic.Models.Infra.ExtendMethods;
using Microsoft.Extensions.Options;
using MSIT147thGraduationTopic.Models.Infra.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using MSIT147thGraduationTopic.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MSIT147thGraduationTopic.Controllers
{
    public class MemberController : Controller
    {
        private readonly GraduationTopicContext _context;
        private IWebHostEnvironment _environment;
        private AddressVM _address;


        protected UserInfoService _userInfoService { get; set; }

        public MemberController(GraduationTopicContext Context
                , UserInfoService UserInfoService, IWebHostEnvironment environment
                , IOptions<OptionSettings> options)
        {
            //DI 指派介面屬性注入指定類別案例。(類別位置在 Program.cs 裡面)
            _context = Context;
            _userInfoService = UserInfoService;
            _environment = environment;
            _address = options.Value.Address;
        }

        public IActionResult CreateMember()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }


        [HttpPost]
        //[Authorize(Roles = "Member")]
        public IActionResult MemberCenter()
        {
            return View(_userInfoService.GetUserInfo());
        }

        [HttpPost]
        //[Authorize(Roles = "Member")]
        public IActionResult ShoppingHistory()
        {
            return View(_userInfoService.GetUserInfo());
        }



        public JObject GetDataFromJsonFile()
        {
            var fileProvider = new PhysicalFileProvider(_environment.WebRootPath);
            var fileInfo = fileProvider.GetFileInfo("~/datas/CityCountyData.json");

            if (fileInfo.Exists)
            {
                using (var stream = fileInfo.CreateReadStream())
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();

                    return JObject.Parse(json);
                }
            }

            return null;
        }



        //載入縣市

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
            //if (city == null) city = "臺中市";

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
