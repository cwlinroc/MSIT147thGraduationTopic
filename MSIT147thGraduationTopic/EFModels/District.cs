﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MSIT147thGraduationTopic.EFModels
{
    public partial class District
    {
        public int DistrictId { get; set; }
        public int ZipCode { get; set; }
        public int CityId { get; set; }
        public string DistrictName { get; set; }

        public virtual City City { get; set; }
    }
}