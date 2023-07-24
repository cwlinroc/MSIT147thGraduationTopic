namespace MSIT147thGraduationTopic.Models.Services
{
    public class UserInfoService
    {
        //宣告私人唯讀資料型別屬性。(Web 站台環境資訊)
        private readonly IHttpContextAccessor _HttpContextAccessor;

        //宣告建構子。(初始化傳入參數為指定型別類別)
        public UserInfoService(IHttpContextAccessor objHttpContextAccessor_Val)
        {
            //DI 指派屬性注入指定類別案例。(類別位置在 Program.cs 裡面)
            _HttpContextAccessor = objHttpContextAccessor_Val;
        }

        /// <summary>
        /// 取得已登入用戶其它欄位資訊方法
        /// </summary>
        public string GetUserInfo_Md()
        {
            //取得 ClaimsIdentity 驗証欄位集合項目。
            var Claim = _HttpContextAccessor.HttpContext.User.Claims.ToList();
            var Email = Claim.Where(a => a.Type == "Email").SingleOrDefault().Value;
            var UserName = Claim.Where(a => a.Type == "UserName").SingleOrDefault().Value;

            //取得 ClaimsIdentity 驗証欄位用戶名稱。
            var Account = _HttpContextAccessor.HttpContext.User.Identity.Name;

            //傳回結果。
            return Email + " = " + Account + " = " + UserName;
        }

    }
}
