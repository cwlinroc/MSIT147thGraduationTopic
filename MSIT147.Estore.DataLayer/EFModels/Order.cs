using System;
using System.Collections.Generic;

namespace MSIT147.Estore.DataLayer.EFModels;

public partial class Order
{
    public int OrderId { get; set; }

    public int MemberId { get; set; }

    public int PaymentMethodId { get; set; }

    public bool Payed { get; set; }

    public DateTime PurchaseTime { get; set; }

    public int? PaymentAmount { get; set; }

    public string DeliveryAddress { get; set; } = null!;

    public string ContactPhoneNumber { get; set; } = null!;

    public string? Remark { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual ICollection<OrderList> OrderLists { get; set; } = new List<OrderList>();

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual ICollection<UsedCoupon> UsedCoupons { get; set; } = new List<UsedCoupon>();
}
