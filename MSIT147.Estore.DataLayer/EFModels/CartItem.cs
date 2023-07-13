using System;
using System.Collections.Generic;

namespace MSIT147.Estore.DataLayer.EFModels;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }

    public int SpecId { get; set; }

    public int Quantity { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Spec Spec { get; set; } = null!;
}
