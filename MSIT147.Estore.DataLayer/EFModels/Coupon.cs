using System;
using System.Collections.Generic;

namespace MSIT147.Estore.DataLayer.EFModels;

public partial class Coupon
{
    public int CouponId { get; set; }

    public int CouponEligibleTagId { get; set; }

    public DateTime CouponStartDate { get; set; }

    public DateTime CouponEndDate { get; set; }

    public int CouponTypeId { get; set; }

    public decimal? CouponDiscountCondition { get; set; }

    public decimal CouponRebate { get; set; }

    public virtual Tag CouponEligibleTag { get; set; } = null!;

    public virtual ICollection<UsedCoupon> UsedCoupons { get; set; } = new List<UsedCoupon>();
}
