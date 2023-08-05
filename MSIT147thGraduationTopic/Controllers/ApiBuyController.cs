using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.Services;
using MSIT147thGraduationTopic.Models.ViewModels;
using NuGet.Packaging.Signing;

namespace MSIT147thGraduationTopic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBuyController : ControllerBase
    {
        private readonly GraduationTopicContext _context;
        private readonly BuyServices _service;

        public ApiBuyController(GraduationTopicContext context)
        {
            _context = context;
            _service = new BuyServices(context);
        }

        [HttpGet("coupons")]
        public async Task<IEnumerable<BuyPageCouponVM>> GetAllCouponsAvalible()
        {
            if (!int.TryParse(HttpContext.User.FindFirstValue("MemberId"), out int memberId))
            {
                return new List<BuyPageCouponVM>();
            }
            var item = (await _service.GetAllCouponsAvalible(memberId));

            return item;
        }

        [HttpGet("cartitems")]
        [HttpGet("cartitems/{couponId}")]
        public async Task<BuyPageCartItemsListVM?> GetCalculatedCartItems(int? couponId)
        {
            string? json = HttpContext.Session.GetString("cartItemIds");
            int[] ids = JsonSerializer.Deserialize<int[]>(json ?? "[]")!;

            return await _service.GetCartItemsWithCoupons(ids, couponId);
        }


        #region --Default參考--
        //// GET: api/ApiBuy
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        //{
        //  if (_context.Orders == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.Orders.ToListAsync();
        //}

        //// GET: api/ApiBuy/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Order>> GetOrder(int id)
        //{
        //  if (_context.Orders == null)
        //  {
        //      return NotFound();
        //  }
        //    var order = await _context.Orders.FindAsync(id);

        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return order;
        //}

        //// PUT: api/ApiBuy/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutOrder(int id, Order order)
        //{
        //    if (id != order.OrderId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(order).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/ApiBuy
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Order>> PostOrder(Order order)
        //{
        //  if (_context.Orders == null)
        //  {
        //      return Problem("Entity set 'GraduationTopicContext.Orders'  is null.");
        //  }
        //    _context.Orders.Add(order);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        //}

        //// DELETE: api/ApiBuy/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOrder(int id)
        //{
        //    if (_context.Orders == null)
        //    {
        //        return NotFound();
        //    }
        //    var order = await _context.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Orders.Remove(order);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool OrderExists(int id)
        //{
        //    return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        //} 
        #endregion
    }
}
