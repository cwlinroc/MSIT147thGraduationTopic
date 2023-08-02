using MSIT147thGraduationTopic.EFModels;
using System.ComponentModel.DataAnnotations;

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
        public int CouponId { get;set; }
        public int? CouponTagId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "優惠券名稱為必填欄位")]
        public string CouponName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "活動開始日期為必填欄位")]
        [DataType(DataType.DateTime, ErrorMessage = "日期格式錯誤，請再次輸入")]
        public DateTime CouponStartDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "活動結束日期為必填欄位")]
        [DataType(DataType.DateTime, ErrorMessage = "日期格式錯誤，請再次輸入")]
        public DateTime CouponEndDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "優惠券名稱為必填欄位")]
        public int CouponDiscountTypeId { get; set; }
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "折扣條件不得為負數")]
        public decimal? CouponCondition { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "優惠券折扣為必填欄位")]
        [Range(0,(double)decimal.MaxValue, ErrorMessage = "折價不得為負數")]
        public decimal CouponDiscount { get; set; }

        //判斷活動日期
        public IEnumerable<ValidationResult> DateTimeValidation(ValidationContext validationContext)
        {
            int result = DateTime.Compare(this.CouponStartDate, this.CouponEndDate);
            if (result >= 0)
            {
                yield return new ValidationResult("活動開始日期不得晚於活動結束日期");
            }
        }

        //判斷打折符合百分比
        public IEnumerable<ValidationResult> CouponConditionValidation(ValidationContext validationContext)
        {
            if(this.CouponCondition == null && CouponDiscount>=100 )
            {
                yield return new ValidationResult("打折數必須一百之內");
            }
        }
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

        static public void UpdateByEditDto(this Coupon coupon, CouponEditDto couponEditDto)
        {
            coupon.CouponId = couponEditDto.CouponId;
            coupon.CouponTagId = couponEditDto.CouponTagId;
            coupon.CouponName = couponEditDto.CouponName;
            coupon.CouponStartDate = couponEditDto.CouponStartDate;
            coupon.CouponEndDate = couponEditDto.CouponEndDate;
            coupon.CouponDiscountTypeId = couponEditDto.CouponDiscountTypeId;
            coupon.CouponCondition = couponEditDto.CouponCondition;
            coupon.CouponDiscount = couponEditDto.CouponDiscount;
        }
    }
}
