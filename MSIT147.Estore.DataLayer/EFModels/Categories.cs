﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MSIT147.Estore.DataLayer.EFModels
{
    public partial class Categories
    {
        public Categories()
        {
            Merchandises = new HashSet<Merchandises>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Merchandises> Merchandises { get; set; }
    }
}