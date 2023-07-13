using System;
using System.Collections.Generic;

namespace MSIT147.Estore.DataLayer.EFModels;

public partial class Cart
{
    public int CartId { get; set; }

    public int MemberId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Member Member { get; set; } = null!;
}
