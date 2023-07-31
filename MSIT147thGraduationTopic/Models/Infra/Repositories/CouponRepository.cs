using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class CouponRepository
    {
        private readonly GraduationTopicContext _context;
        public CouponRepository(GraduationTopicContext context)
        {
            _context = context;
        }

        public IEnumerable<CouponDto> GetAllCoupons()
        {
            var coupons = _context.Coupons.ToList();
            return coupons.Select(c => c.ToDto());
        }

        public IEnumerable<CouponDto> ShowCoupons(int id)
        {
            var coupons = _context.Coupons.Where(c=>c.CouponDiscountTypeId == id ).ToList();
            return coupons.Select(c => c.ToDto());
        }

        public int CreateCoupon(CouponDto cDto)
        {
            var obj = cDto.ToEF();
            _context.Coupons.Add(obj);
            _context.SaveChanges();
            return obj.CouponId;
        }

        public int EditCoupon(CouponEditDto cEDto,int couponId)
        {
            var coupon = _context.Coupons.FirstOrDefault(c=>c.CouponId == couponId);
            if (coupon == null)
            {
                return -1;
            }
            coupon.UpdateByEditDto(cEDto);
            
            _context.SaveChanges();
            return couponId;
        }

        public int DeleteCoupon(int couponId)
        {
            var coupon = _context.Coupons.Find(couponId);
            if(coupon == null)
            {
                return -1;
            }
            
            _context.Coupons.Remove(coupon);

            _context.SaveChanges();
            return couponId;
        }

        //public int DetectionName(string name)
        //{
        //    var coupon = _context.Coupons.FirstOrDefault(c=>c.CouponName == name);

        //    if(coupon != null)
        //    {

        //    }
        //}
    }
}
