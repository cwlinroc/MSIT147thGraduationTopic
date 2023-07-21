using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Services;
using MSIT147thGraduationTopic.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace MSIT147thGraduationTopic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiCartController : ControllerBase
    {
        private readonly GraduationTopicContext _context;
        private readonly CartService _service;

        public ApiCartController(GraduationTopicContext context)
        {
            _context = context;
            _service = new CartService(context);
        }


        [HttpGet]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CartItemDisplayDto>>> GetCartItem(int id = 5)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var cartItems = await _service.GetCartItemsByMeberId(id);

            return cartItems ?? null;
        }

        // PUT: api/ApiCart
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutCartItem(CartItemDto cartItem)
        {
            try
            {
                await _service.ChangeCartItemQuantity(cartItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        //POST: api/ApiCart
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CartItem>> PostCartItem(CartIVM vm)
        {
            //HttpContext.Session.Set<object>(key, value);
            if (vm.CartItemIds == null || vm.CartItemIds.Length == 0) return BadRequest(ModelState);

            HttpContext.Session.SetString("cartItemIds", JsonSerializer.Serialize(vm.CartItemIds));

            return Redirect(Url.Content("~/buy/index"));
        }

        // DELETE: api/ApiCart/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            await _service.DeleteCartItem(id);

            return NoContent();
        }

    }
}
