using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Services;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Data;
using System.Security.Claims;
using Newtonsoft.Json;

namespace MSIT147thGraduationTopic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiMemberController : ControllerBase
    {
        private readonly GraduationTopicContext _context;
        private readonly MemberService _service;
        private readonly IWebHostEnvironment _environment;
        protected UserInfoService _userInfoService { get; set; }

        public ApiMemberController(GraduationTopicContext context
            , IWebHostEnvironment environment, UserInfoService UserInfoService)
        {
            _context = context;
            _environment = environment;
            _service = new MemberService(context, environment);
            _userInfoService = UserInfoService;
        }

        [HttpGet]
        public ActionResult<List<MemberVM>> GetAllMembers()
        {
            return _service.GetAllMembers().ToList();
        }

        [HttpGet("{query}")]
        public ActionResult<List<MemberVM>> GetMembersByNameOrAccount(string query)
        {
            return _service.GetMembersByNameOrAccount(query).ToList();
        }

        [HttpPost]
        public ActionResult<int> CreateMember([FromForm] MemberCreateVM vm, [FromForm] IFormFile avatar)
        {
            var memberId = _service.CreateMember(vm.ToDto(), avatar);

            return memberId;
        }

        [HttpPut("{id}")]
        public ActionResult<int> UpdateMember([FromForm] MemberEditDto dto, int id, [FromForm] IFormFile avatar)
        {
            var memberId = _service.EditMember(dto, id, avatar);

            return memberId;
        }

        [HttpDelete("{id}")]
        public ActionResult<int> UpdateMember(int id)
        {
            return _service.DeleteMember(id);
        }

        /// <summary>
        /// 宣告公開動作結果方法。(登入處理)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(Models.ViewModels.LogInVM formdata)
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

        /// <summary>
        /// 宣告公開動作結果方法。(已登入用戶表單頁面)
        /// </summary>
        [Authorize] //檢查用戶必須登入
        public IActionResult UserForm()
        {
            //傳回文字內容結果。(已登入的用戶名稱) (在 Controller 範圍內使用方式)
            return Content(HttpContext.User.Identity.Name);
        }

        /// <summary>
        /// 宣告公開動作結果方法。(取得已登入用戶其它欄位資訊頁面)
        /// </summary>
        [Authorize] //檢查用戶必須登入
        public IActionResult UserInfoForm()
        {
            //HttpContext.User.Claims

            //傳回檢視頁面結果。(取得已登入用戶其它欄位資訊) (在 Controller 範圍外使用方式)
            return Content(_userInfoService.GetUserInfo());
        }

        /// <summary>
        /// 宣告公開動作結果方法。(已登入指定角色用戶表單頁面)
        /// </summary>
        [Authorize(Roles = "Admin,Manager")] //檢查用戶角色是否為指定角色名稱的屬性類別
        public IActionResult AdminForm()
        {
            //HttpContext.User.Claims

            //傳回檢視頁面結果。(取得已登入用戶其它欄位資訊)
            return Content(_userInfoService.GetUserInfo());
        }

        /// <summary>
        /// 宣告公開動作結果方法。(未登入處理)
        /// </summary>
        public IActionResult NoLogin()
        {
            return Content("尚未登入");
        }

        /// <summary>
        /// 宣告公開動作結果方法。(用戶角色沒有權限處理)
        /// </summary>
        public IActionResult NoRole()
        {
            return Content("沒有權限");
        }
        
        

        
    }
}
