using System;
using System.Collections.Generic;

namespace MSIT147.Estore.DataLayer.EFModels;

public partial class MerchandiseTag
{
    public int MerchandiseId { get; set; }

    public int TagId { get; set; }

    public virtual Merchandise Merchandise { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
