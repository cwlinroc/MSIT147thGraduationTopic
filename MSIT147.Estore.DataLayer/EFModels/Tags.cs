﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MSIT147.Estore.DataLayer.EFModels
{
    public partial class Tags
    {
        public Tags()
        {
            Coupons = new HashSet<Coupons>();
        }

        public int TagId { get; set; }
        public string TagName { get; set; }

        public virtual ICollection<Coupons> Coupons { get; set; }
    }
}