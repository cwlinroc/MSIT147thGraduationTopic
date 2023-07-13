using System;
using System.Collections.Generic;

namespace MSIT147.Estore.DataLayer.EFModels;

public partial class CouponOwner
{
    public int CouponId { get; set; }

    public string CouponSerialNumber { get; set; } = null!;

    public int MemberId { get; set; }

    public virtual Coupon Coupon { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
