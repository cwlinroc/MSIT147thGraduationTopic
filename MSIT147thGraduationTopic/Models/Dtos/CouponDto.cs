using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Models.Dtos
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        public int CouponEligibleTagId { get; set; }
        public DateTime CouponStartDate { get; set; }
        public DateTime CouponEndDate { get; set; }
        public int CouponTypeId { get; set; }
        public decimal? CouponDiscountCondition { get; set; }
        public decimal CouponRebate { get; set; }
    }

    static public class CouponConverter
    {
        static public CouponDto ToDto(this Coupon coupon)
        {
            return new CouponDto
            {
                CouponId = coupon.CouponId,
                CouponEligibleTagId = coupon.CouponEligibleTagId,
                CouponStartDate = coupon.CouponStartDate,
                CouponDiscountCondition = coupon.CouponDiscountCondition,
                CouponEndDate = coupon.CouponEndDate,
                CouponRebate = coupon.CouponRebate,
                CouponTypeId = coupon.CouponTypeId,
            };
        }
    }

}
