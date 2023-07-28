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

namespace MSIT147thGraduationTopic.Controllers
{
    public class MemberController : Controller
    {
        private readonly GraduationTopicContext _context;
        private IWebHostEnvironment _environment;

        
        protected UserInfoService _userInfoService { get; set; }
        
        public MemberController(GraduationTopicContext Context
                , UserInfoService UserInfoService, IWebHostEnvironment environment)
        {
            //DI 指派介面屬性注入指定類別案例。(類別位置在 Program.cs 裡面)
            _context = Context;
            _userInfoService = UserInfoService;
            _environment = environment;
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

        //AddressVM.Record _addressdata = new AddressVM.Record();

        //public AddressVM.Record GetDataFromJsonFile()
        //{
        //    var fileProvider = new PhysicalFileProvider(_environment.WebRootPath);
        //    var fileInfo = fileProvider.GetFileInfo("datas/111address.json");

        //    if (fileInfo.Exists)
        //    {
        //        using (var stream = fileInfo.CreateReadStream())
        //        using (var reader = new StreamReader(stream))
        //        {
        //            var json = reader.ReadToEnd();
        //            _addressdata = JsonConvert.DeserializeObject<AddressVM.Record>(json);
        //            return _addressdata;
        //        }
        //    }

        //    return null;
        //}

        ////載入縣市
        //[NonAction]
        //public IActionResult Cities()
        //{
        //    var cities = _addressdata.Select(a => a.city).Distinct();
        //    return Json(cities);
        //}

        ////根據縣市載入鄉鎮區
        //[NonAction]
        //public IActionResult Districts(string city)
        //{
        //    var district = _addressdata.Where(a => a.city == city)
        //        .Select(a => a.site_id).Distinct();
        //    return Json(district);
        //}
    }
}
