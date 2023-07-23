using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Services;

namespace MSIT147thGraduationTopic.Controllers
{
    public class MemberFrontController : Controller
    {
        //宣告私人唯讀物件型別屬性。(DB 資料集合)
        private readonly GraduationTopicContext _context;

        //宣告不公開物件型別屬性。(文章資料服務)
        protected UserInfoService _UserInfoService { get; set; }

        //宣告建構子。(初始化傳入參數為指定類別型別)
        public MemberFrontController(
            GraduationTopicContext Context,
            UserInfoService UserInfoService)
        {
            //DI 指派介面屬性注入指定類別案例。(類別位置在 Program.cs 裡面)
            _context = Context;
            _UserInfoService = UserInfoService;
        }
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Register(Models.ViewModels.MemberRegisterVM formdata)
        {
            
            return View();
        }

        /// <summary>
        /// 宣告公開動作結果方法。(登入頁面)
        /// </summary>
        public IActionResult LogIn()
        {

            return View();
        }

        /// <summary>
        /// 宣告公開動作結果方法。(登入處理)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(Models.ViewModels.LogInVM formdata)
        {
            //依據帳號與密碼檢索取得此登入用戶記錄。
            var User = (from a in _context.Members
                        where a.Account == formdata.Account
                        && a.Password == formdata.Password
                        select a).SingleOrDefault();
            return View();
        }

        public IActionResult LogOut()
        {

            return View();
        }

        

        [HttpPost]
        [Authorize(Roles = "Member")]
        public IActionResult MemberCenter(Member member)
        {
            return View();
        }
        

        

        [HttpPost]
        [Authorize(Roles = "Member")]
        public IActionResult ShoppingHistory(Member member)
        {
            return View();
        }
    }
}
