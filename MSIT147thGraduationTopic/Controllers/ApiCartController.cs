using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiCartController : ControllerBase
    {
        private readonly GraduationTopicContext _context;

        public ApiCartController(GraduationTopicContext context)
        {
            _context = context;
        }

        // GET: api/ApiCart
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems(int memberId)
        {
          if (_context.CartItems == null)
          {
              return NotFound();
          }
            return await _context.CartItems.ToListAsync();
        }

        // GET: api/ApiCart/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartItem>> GetCartItem(int id)
        {
          if (_context.CartItems == null)
          {
              return NotFound();
          }
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            return cartItem;
        }

        // PUT: api/ApiCart/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItem(int id, CartItem cartItem)
        {
            if (id != cartItem.CartItemId)
            {
                return BadRequest();
            }

            _context.Entry(cartItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ApiCart
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CartItem>> PostCartItem(CartItem cartItem)
        {
          if (_context.CartItems == null)
          {
              return Problem("Entity set 'GraduationTopicContext.CartItems'  is null.");
          }
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCartItem", new { id = cartItem.CartItemId }, cartItem);
        }

        // DELETE: api/ApiCart/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            if (_context.CartItems == null)
            {
                return NotFound();
            }
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartItemExists(int id)
        {
            return (_context.CartItems?.Any(e => e.CartItemId == id)).GetValueOrDefault();
        }
    }
}
