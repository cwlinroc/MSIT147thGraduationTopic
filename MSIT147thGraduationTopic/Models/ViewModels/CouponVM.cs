using MSIT147thGraduationTopic.Models.Dtos;
using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class CouponVM
    {

        public int CouponId { get; set; }
        public int? CouponTagId { get; set; }
        public string CouponName { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CouponStartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CouponEndDate { get; set; }
        public int CouponDiscountTypeId { get; set; }
        public decimal? CouponCondition { get; set; }
        public decimal CouponDiscount { get; set; }
    }

    public class CouponCreateVM
    {
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

    public class CouponReceiveCreateVM
    {
        public int MemberId { get; set; }
        public int CouponId { get; set; }
        public string CouponName { get; set; }
        public int? CouponTagId { get; set; }
        public DateTime CouponStartDate { get; set; }
        public DateTime CouponEndDate { get; set; }
        public int CouponDiscountTypeId { get; set; }
        public decimal? CouponCondition { get; set; }
        public decimal CouponDiscount { get; set; }
        public bool CouponUsed { get; set; }
    }


    //public class CouponEditVM
    //{
    //    public int? CouponTagId { get; set; }
    //    [Required(AllowEmptyStrings = false, ErrorMessage = "優惠券名稱為必填欄位")]
    //    public string CouponName { get; set; }
    //    [Required(AllowEmptyStrings = false, ErrorMessage = "活動開始日期為必填欄位")]
    //    [DataType(DataType.DateTime, ErrorMessage = "日期格式錯誤，請再次輸入")]
    //    public DateTime CouponStartDate { get; set; }
    //    [Required(AllowEmptyStrings = false, ErrorMessage = "活動結束日期為必填欄位")]
    //    [DataType(DataType.DateTime, ErrorMessage = "日期格式錯誤，請再次輸入")]
    //    public DateTime CouponEndDate { get; set; }
    //    [Required(AllowEmptyStrings = false, ErrorMessage = "優惠券名稱為必填欄位")]
    //    public int CouponDiscountTypeId { get; set; }
    //    [Range(0, (double)decimal.MaxValue, ErrorMessage = "折扣條件不得為負數")]
    //    public decimal? CouponCondition { get; set; }
    //    [Required(AllowEmptyStrings = false, ErrorMessage = "優惠券折扣為必填欄位")]
    //    [Range(0, (double)decimal.MaxValue, ErrorMessage = "折價不得為負數")]
    //    public decimal CouponDiscount { get; set; }

    //    //判斷活動日期
    //    public IEnumerable<ValidationResult> DateTimeValidation(ValidationContext validationContext)
    //    {
    //        int result = DateTime.Compare(this.CouponStartDate, this.CouponEndDate);
    //        if (result >= 0)
    //        {
    //            yield return new ValidationResult("活動開始日期不得晚於活動結束日期");
    //        }
    //    }

    //    //判斷打折符合百分比
    //    public IEnumerable<ValidationResult> CouponConditionValidation(ValidationContext validationContext)
    //    {
    //        if (this.CouponCondition == null && CouponDiscount >= 100)
    //        {
    //            yield return new ValidationResult("打折數必須一百之內");
    //        }
    //    }
    //}

    static public class CouponVMTransfer
    {
        static public CouponVM ToVM(this CouponDto cDto)
        {
            return new CouponVM
            {
                CouponId = cDto.CouponId,
                CouponTagId = cDto.CouponTagId,
                CouponName = cDto.CouponName,
                CouponStartDate = cDto.CouponStartDate,
                CouponEndDate = cDto.CouponEndDate,
                CouponDiscountTypeId = cDto.CouponDiscountTypeId,
                CouponCondition = cDto.CouponCondition,
                CouponDiscount = cDto.CouponDiscount,
            };
        }

        static public CouponDto ToDto(this CouponCreateVM cVM)
        {
            return new CouponDto
            {
                CouponTagId = cVM.CouponTagId,
                CouponName = cVM.CouponName,
                CouponStartDate = cVM.CouponStartDate,
                CouponEndDate = cVM.CouponEndDate,
                CouponDiscountTypeId = cVM.CouponDiscountTypeId,
                CouponCondition = cVM.CouponCondition,
                CouponDiscount = cVM.CouponDiscount,
            };
        }
    }

    static public class CouponReceiveVMTransfer
    {
        static public CouponReceiveCreateVM ToReceiveVM(this CouponFrontDto cFDto)
        {
            return new CouponReceiveCreateVM
            {
                MemberId = cFDto.MemberId,
                CouponId = cFDto.CouponId,
                CouponTagId = cFDto.CouponTagId,
                CouponName = cFDto.CouponName,
                CouponStartDate = cFDto.CouponStartDate,
                CouponEndDate = cFDto.CouponEndDate,
                CouponDiscountTypeId = cFDto.CouponDiscountTypeId,
                CouponCondition = cFDto.CouponCondition,
                CouponDiscount = cFDto.CouponDiscount,
                CouponUsed = cFDto.CouponUsed,
            };
        }

        static public CouponFrontDto ToCFDto(this CouponReceiveCreateVM cRCVM)
        {
            return new CouponFrontDto
            {
                MemberId = cRCVM.MemberId,
                CouponId = cRCVM.CouponId,
                CouponTagId = cRCVM.CouponTagId,
                CouponName = cRCVM.CouponName,
                CouponStartDate = cRCVM.CouponStartDate,
                CouponEndDate = cRCVM.CouponEndDate,
                CouponDiscountTypeId = cRCVM.CouponDiscountTypeId,
                CouponCondition = cRCVM.CouponCondition,
                CouponDiscount = cRCVM.CouponDiscount,
                CouponUsed = cRCVM.CouponUsed,
            };
        }
    }
}

