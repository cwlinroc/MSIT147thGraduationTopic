using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Models.Dtos
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        public int CouponTagId { get; set; }
        public DateTime CouponStartDate { get; set; }
        public DateTime CouponEndDate { get; set; }
        public int CouponTypeId { get; set; }
        public decimal? CouponCondition { get; set; }
        public decimal CouponDiscount { get; set; }
    }

    static public class CouponConverter
    {
        static public CouponDto ToDto(this Coupon coupon)
        {
            return new CouponDto
            {
                CouponId = coupon.CouponId,
                CouponTagId = coupon.CouponTagId,
                CouponStartDate = coupon.CouponStartDate,
                CouponCondition = coupon.CouponCondition,
                CouponEndDate = coupon.CouponEndDate,
                CouponDiscount = coupon.CouponDiscount,
                CouponTypeId = coupon.CouponDiscountTypeId,
            };
        }
    }

}
