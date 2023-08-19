using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using MSIT147thGraduationTopic.EFModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using MSIT147thGraduationTopic.Models.Services;
using System.Security.Policy;
using static MSIT147thGraduationTopic.Models.Infra.Utility.MailSetting;
using MSIT147thGraduationTopic.Models.Infra.Utility;
using MSIT147thGraduationTopic.Models.Infra.ExtendMethods;
using MSIT147thGraduationTopic.Models.Interfaces;
using Google.Apis.Auth;

namespace MSIT147thGraduationTopic.Controllers.Member
{
    public class MemberController : Controller
    {
        private readonly GraduationTopicContext _context;
        private readonly MemberService _service;
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _environment;
        private readonly IUrlHelper _url;


        public MemberController(GraduationTopicContext context, IUrlHelper url, IMailService mailService,
                 IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _url = url;
            _service = new MemberService(context, environment);
            _mailService = mailService;
        }

        public IActionResult CreateMember()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }


        public IActionResult ForgetPwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPwd(string account, string email)
        {
            try
            {
                var member = _context.Members.FirstOrDefault(a => a.Account == account);
                string body = _mailService.CreateUrl(member.Account, _url, "EmailVerify", "Member");

                if (member == null || email != member.Email)
                {
                    ViewBag.ErrorMessage = "帳號不存在或無效!";
                    return View();
                }

                MailRequest request = new MailRequest()
                {
                    ToEmail = member.Email,
                    Subject = "福祿獸購物商城帳號驗證信",
                    Body = $"<html><body><h1>驗證確認</h1><h3><a href=\"{body}\">請點這裡驗證</a></h3></body></html>"
                };

                _mailService.SendEmailAsync(request);
                ViewBag.SuccessMessage = "郵件已發送，請檢查您的信箱!";
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult EmailVerify(string token)
        {   // 在此查找資料庫中與此token相匹配的帳戶
            var member = _context.Members.FirstOrDefault(a => a.ConfirmGuid == token);

            if (member != null)
            {
                // 轉到改密碼頁面
                return RedirectToAction("EmailVT", new { account = member.Account });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult EmailVT(string account)
        {
            var member = _context.Members.FirstOrDefault(a => a.Account == account);

            if (!member.IsActivated)
            {
                member.ConfirmGuid = null;
                member.IsActivated = true;
                _context.SaveChanges();

                return RedirectToAction("CertificateBulletin", "Member");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmailVT(string account, string password)
        {
            var member = _context.Members.FirstOrDefault(a => a.Account == account);

            if (member != null)
            {
                var salt = new RandomGenerator().RandomSalt();

                // 如果找到了相匹配的帳戶，更新帳戶狀態                
                member.ConfirmGuid = null; //清空token
                member.Salt = salt;
                member.Password = password.GetSaltedSha256(salt);
                _context.SaveChanges();
                
                Thread.Sleep(3000);
                return RedirectToAction("Index", "Home");
                //return View();
            }

            ViewBag.ErrorMessage = "存取失敗，請重新操作。";
            return View();
        }

        public ActionResult CertificateBulletin()
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
