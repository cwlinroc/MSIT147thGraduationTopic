using Dapper;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.ViewModels;

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

        

        public IEnumerable<CouponUsedStatus> GetReceivableCoupon(int memberId)
        {
            var activateCoupons = _context.Coupons.Where(c => c.CouponStartDate < DateTime.Now
                && c.CouponEndDate > DateTime.Now).Select(c=>c.toUsedStatusDto()).ToList();
            var hasReceive = _context.CouponOwners.Where(c => c.MemberId == memberId)
                .Select(c => new CouponUsedStatus
                {
                    CouponId = c.CouponId,
                    CouponName = c.Coupon.CouponName,
                    CouponStartDate = c.Coupon.CouponStartDate,
                    CouponEndDate =c.Coupon.CouponEndDate,
                    UsedStatus = (c.CouponUsed) ? 3 : 2
                }).ToList();

            foreach (var coupon in activateCoupons)
            {
                if (hasReceive.Select(c => c.CouponId).Contains(coupon.CouponId))
                {
                    coupon.UsedStatus = 2;
                }
                else
                {
                    coupon.UsedStatus = 1;
                }
            }
            var result = activateCoupons
                .Where(c=>!hasReceive.Select(o=>o.CouponId).Contains(c.CouponId))
                .Concat(hasReceive);
            return result;
        }

        public IEnumerable<CouponDto> ShowCoupons(int id)
        {
            var coupons = _context.Coupons.Where(c => c.CouponDiscountTypeId == id);

            return coupons.ToList().Select(c => c.ToDto());
        }

        public int CreateCoupon(CouponDto cDto)
        {
            var obj = cDto.ToEF();
            _context.Coupons.Add(obj);
            _context.SaveChanges();
            return obj.CouponId;
        }

        public int CouponReceive(int memberId, int couponId)
        {
            //var couponOwner = new CouponOwner { 
            //    MemberId = memberId,
            //    CouponId = couponId,
            //    CouponUsed = false,
            //};
            //_context.CouponOwners.Add(couponOwner);
            //return _context.SaveChanges();
            var conn = _context.Database.GetDbConnection();
            string sql = "INSERT INTO CouponOwners ( CouponId , MemberId , CouponUsed )" +
                " values (@CouponId , @MemberId , @CouponUsed )";
            return conn.Execute(sql, new { MemberId = memberId, CouponId = couponId, CouponUsed = false });

        }

        public int EditCoupon(CouponEditDto cEDto)
        {
            var couponData = _context.Coupons.FirstOrDefault(c => c.CouponId == cEDto.CouponId);

            if (couponData == null)
            {
                return -1;
            }
            //couponData.ChangeByDto(ceDto)

            couponData.CouponName = cEDto.CouponName;
            couponData.CouponTagId = cEDto.CouponTagId;
            couponData.CouponStartDate = cEDto.CouponStartDate;
            couponData.CouponEndDate = cEDto.CouponEndDate;
            couponData.CouponDiscount = cEDto.CouponDiscount;
            couponData.CouponCondition = cEDto.CouponCondition;

            _context.Coupons.Update(couponData);
            _context.SaveChanges();
            return couponData.CouponId;
        }

        public int DeleteCoupon(int couponId)
        {
            var coupon = _context.Coupons.Find(couponId);
            if (coupon == null)
            {
                return -1;
            }

            _context.Coupons.Remove(coupon);

            _context.SaveChanges();
            return couponId;
        }

        public CouponDto GetCouponById(int id)
        {
            // 通過id搜尋單筆資料
            var coupon = _context.Coupons.FirstOrDefault(c => c.CouponId == id);
            if (coupon == null)
            {
                // 未搜尋到id時的處理
                return null;
            }

            return coupon.ToDto(); //將coupon實體轉換為couponDto
        }

        public CouponFrontDto GetCouponByMemberID(int id)
        {
            var coupon = _context.CouponReceives.FirstOrDefault(c => c.MemberId == id);
            if (coupon == null)
            {
                return null;
            }
            return coupon.ToFrontDto();
        }
    }
}
