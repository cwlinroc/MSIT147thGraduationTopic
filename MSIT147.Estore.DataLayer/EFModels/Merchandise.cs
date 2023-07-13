using System;
using System.Collections.Generic;

namespace MSIT147.Estore.DataLayer.EFModels;

public partial class Merchandise
{
    public int MerchandiseId { get; set; }

    public string MerchandiseName { get; set; } = null!;

    public int BrandId { get; set; }

    public int CategoryId { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool Display { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();

    public virtual ICollection<Spec> Specs { get; set; } = new List<Spec>();
}
