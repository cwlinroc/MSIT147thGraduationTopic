using System;
using System.Collections.Generic;

namespace MSIT147.Estore.DataLayer.EFModels;

public partial class Evaluation
{
    public int EvaluationId { get; set; }

    public int MerchandiseId { get; set; }

    public int MemberId { get; set; }

    public int Score { get; set; }

    public string? Comment { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual Merchandise Merchandise { get; set; } = null!;
}
