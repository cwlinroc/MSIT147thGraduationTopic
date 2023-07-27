using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Services;
using System.Security.Claims;
using MSIT147thGraduationTopic.Models.Infra.ExtendMethods;
using Microsoft.Extensions.Options;
using MSIT147thGraduationTopic.Models.Infra.Utility;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Register()
        {
            return View();
        }  
        
        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult ForgetPwd()
        {

            //依據帳號與密碼檢索取得此登入用戶記錄。            
            var emp = (from a in _context.Employees
                       where a.EmployeeAccount == formdata.Account
                       && a.EmployeePassword == formdata.Password
                       select a).SingleOrDefault();
            //沒有找到此員工記錄時
            if (emp == null)
            {
                var member = (from a in _context.Members
                              where a.Account == formdata.Account
                              && a.Password == formdata.Password
                              select a).SingleOrDefault();
                //沒有找到此會員記錄時
                if (member == null)
                {
                    //傳回文字
                    return Content("帳號密碼錯誤");
                }
                else  //有找到會員時
                {
                    //依據 Identity 要求 Claim 類別格式對應指派登入成功的用戶資料。
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, member.Account), //帳號名稱 (規定名稱)
                            new Claim("MemberName", member.MemberName), //用戶全名 (自訂名稱)
                            new Claim(ClaimTypes.Email, member.Email), //帳號電子信箱 (規定名稱)
                            new Claim(ClaimTypes.Role, "Member")  //用戶角色 (規定名稱)
                        };

                    ////初始化 Claim 類別的新執行個體。(角色資料集合)
                    //foreach (var temp in role)
                    //{
                    //    //加入角色資料
                    //    claims.Add(new Claim(ClaimTypes.Role, temp.Role_F));
                    //}

                    //建構 ClaimsIdentity Cookie 用戶驗証物件的狀態存取案例。
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    //執行 ClaimsIdentity Cookie 用戶驗証物件的操作登入動作。(使用 Cookie 操作內部驗証狀態控管與流程執行  )
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    //===== AspNetCore.Authentication 單一範圍的驗証機制組態設置 =====
                    //HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(varClaimsIdentity), authProperties);
                }

                //傳回文字內容訊息。
                return Content("登入成功!");
            }
            else //有找到員工時
            {
                //依據 Identity 要求 Claim 類別格式對應指派登入成功的用戶資料。
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, emp.EmployeeAccount), //帳號名稱 (規定名稱)
                        new Claim("EmployeeName", emp.EmployeeName), //用戶全名 (自訂名稱)
                        new Claim(ClaimTypes.Email, emp.EmployeeEmail), //帳號電子信箱 (規定名稱)                        
                    };

                //===== AspNetCore.Authentication 角色驗証機制項目設置 =====
                //在角色名稱尋找取得該用戶的資料。
                //var role = from a in _context.Role
                //           where a.EmployeeId == emp.EmployeeId
                //           select a;

                //初始化 Claim 類別的新執行個體。(角色資料集合)
                //foreach (var temp in role)
                //{
                //    //加入角色資料
                //    claims.Add(new Claim(ClaimTypes.Role, temp.Role_F));
                //}
            }

            //傳回文字內容訊息。
            return Content("登入成功!");

        }

        /// <summary>
        /// 宣告公開動作結果方法。(登出處理)
        /// </summary>
        public IActionResult LogOut()
        {
            //執行 ClaimsIdentity Cookie 用戶驗証物件的操作登出動作。
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //傳回文字內容訊息。
            return Content("登出");
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

        AddressVM.Record _addressdata = new AddressVM.Record();

        public AddressVM.Record GetDataFromJsonFile()
        {
            var fileProvider = new PhysicalFileProvider(_environment.WebRootPath);
            var fileInfo = fileProvider.GetFileInfo("datas/111address.json");

            if (fileInfo.Exists)
            {
                using (var stream = fileInfo.CreateReadStream())
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    _addressdata = JsonConvert.DeserializeObject<AddressVM.Record>(json);
                    return _addressdata;
                }
            }

            return null;
        }

        //載入縣市
        [NonAction]
        public IActionResult Cities()
        {
            var cities = _addressdata.Select(a => a.city).Distinct();
            return Json(cities);
        }

        //根據縣市載入鄉鎮區
        [NonAction]
        public IActionResult Districts(string city)
        {
            var district = _addressdata.Where(a => a.city == city)
                .Select(a => a.site_id).Distinct();
            return Json(district);
        }
    }
}
