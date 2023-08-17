using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;
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
        public int CouponId { get; set; }
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
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "折價不得為負數")]
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
            if (this.CouponCondition == null && CouponDiscount >= 100)
            {
                yield return new ValidationResult("打折數必須一百之內");
            }
        }
    }

    public class CouponFrontDto
    {
        public int MemberId { get; set; }
        public int CouponId { get; set; }
        public int? CouponTagId { get; set; }
        public string CouponName { get; set; }
        public DateTime CouponStartDate { get; set; }
        public DateTime CouponEndDate { get; set; }
        public int CouponDiscountTypeId { get; set; }
        public decimal? CouponCondition { get; set; }
        public decimal CouponDiscount { get; set; }
        public bool CouponUsed { get; set; }
    }

    public class CouponUsedStatus
    {
        public int CouponId { get; set; }
        public string CouponName { get;set; }
        public DateTime CouponStartDate { get;set; }
        public DateTime CouponEndDate { get;set; }
        public int UsedStatus { get; set; }
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

        static public CouponFrontDto ToFrontDto(this CouponReceive couponReceive)
        {
            return new CouponFrontDto
            {
                MemberId = couponReceive.MemberId,
                CouponId = couponReceive.CouponId,
                CouponName = couponReceive.CouponName,
                CouponStartDate = couponReceive.CouponStartDate,
                CouponEndDate = couponReceive.CouponEndDate,
                CouponCondition = couponReceive.CouponCondition,
                CouponDiscount = couponReceive.CouponDiscount,
                CouponDiscountTypeId = couponReceive.CouponDiscountTypeId,
                CouponTagId = couponReceive.CouponTagId,
                CouponUsed= couponReceive.CouponUsed,
            };
        }

        static public CouponUsedStatus toUsedStatusDto(this Coupon coupon)
        {
            return new CouponUsedStatus
            {
                CouponId = coupon.CouponId,
                CouponName = coupon.CouponName,
                CouponStartDate= coupon.CouponStartDate,
                CouponEndDate = coupon.CouponEndDate,
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
    }
}
