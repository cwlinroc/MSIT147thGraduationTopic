using System;
using System.Collections.Generic;

namespace MSIT147.Estore.DataLayer.EFModels;

public partial class UsedCoupon
{
    public int UsedCouponId { get; set; }

    public int CouponId { get; set; }

    public int OrderId { get; set; }

    public virtual Coupon Coupon { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
