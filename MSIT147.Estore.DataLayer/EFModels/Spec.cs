using System;
using System.Collections.Generic;

namespace MSIT147.Estore.DataLayer.EFModels;

public partial class Spec
{
    public int SpecId { get; set; }

    public string SpecName { get; set; } = null!;

    public int MerchandiseId { get; set; }

    public int Price { get; set; }

    public int Amount { get; set; }

    public int DisplayOrder { get; set; }

    public bool OnShelf { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Merchandise Merchandise { get; set; } = null!;

    public virtual ICollection<OrderList> OrderLists { get; set; } = new List<OrderList>();
}
