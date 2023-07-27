using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Models.Dtos
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        public int? CouponTagId { get; set; }
        public string CouponName { get; set; }
        public DateTime CouponStartDate { get; set; }
        public DateTime CouponEndDate { get; set; }
        public int CouponDiscountTypeId { get; set; }
        public decimal? CouponCondition { get; set; }
        public decimal CouponDiscount { get; set; }
    }
    public class CouponEditDto
    {
        public string CouponName { get; set; }
        public DateTime CouponStartDate { get; set; }
        public DateTime CouponEndDate { get; set; }
        public int CouponDiscountTypeId { get; set; }
        public decimal? CouponCondition { get; set; }
        public decimal CouponDiscount { get; set; }
    }

    static public class CouponConvert
    {
        static public CouponDto ToDto(this Coupon coupon)
        {
            return new CouponDto
            {
                CouponId = coupon.CouponId,
                CouponTagId = coupon.CouponTagId,
                CouponName = coupon.CouponName,
                CouponStartDate = coupon.CouponStartDate,
                CouponCondition = coupon.CouponCondition,
                CouponEndDate = coupon.CouponEndDate,
                CouponDiscount = coupon.CouponDiscount,
                CouponDiscountTypeId = coupon.CouponDiscountTypeId,
            };
        }

        static public Coupon ToEF(this CouponDto couponDto)
        {
            return new Coupon
            {
                CouponId = couponDto.CouponId,
                CouponTagId = couponDto.CouponTagId,
                CouponName = couponDto.CouponName,
                CouponStartDate = couponDto.CouponStartDate,
                CouponCondition = couponDto.CouponCondition,
                CouponEndDate = couponDto.CouponEndDate,
                CouponDiscount = couponDto.CouponDiscount,
                CouponDiscountTypeId = couponDto.CouponDiscountTypeId,
            };
        }

        static public void UpdateByEditDto(this Coupon coupon, CouponDto couponDto)
        {
            coupon.CouponName = couponDto.CouponName;
            coupon.CouponStartDate = couponDto.CouponStartDate;
            coupon.CouponEndDate = couponDto.CouponEndDate;
            coupon.CouponDiscountTypeId = couponDto.CouponDiscountTypeId;
            coupon.CouponCondition = couponDto.CouponCondition;
            coupon.CouponDiscount = couponDto.CouponDiscount;
        }
    }

}
