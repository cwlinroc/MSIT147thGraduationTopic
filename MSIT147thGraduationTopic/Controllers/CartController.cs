using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.Models.ViewModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index(int id = 5)
        {
            return View(id);
        }

        //[HttpPost]
        //public IActionResult Index([FromBody] CartIVM vm)
        //{
        //    if (vm.CartItemIds == null || vm.CartItemIds.Length == 0) return BadRequest(ModelState);

        //    //Response.Headers.Add("REQUIRES_AUTH", "1");

        //    string ids = String.Join("&", vm.CartItemIds.Select(o=> "ids=" + o));

        //    return Redirect(Url.Content($"~/buy/index?" + ids));
        //}


        //[HttpPost]
        //public IActionResult Index([FromBody] string test)
        //{

        //    ////Response.Headers.Add("REQUIRES_AUTH", "1");
        //    //if (vm == null || String.IsNullOrEmpty(vm.test)) return BadRequest(ModelState);

        //    //return Redirect(Url.Content($"~/buy/index") + vm.test);
        //    if (String.IsNullOrEmpty(test)) return BadRequest(ModelState);

        //    return Redirect(Url.Content($"~/buy/index") + test);


        //}


    }
}
