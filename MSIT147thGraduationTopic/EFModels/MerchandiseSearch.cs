﻿using System;
using System.Collections.Generic;

namespace MSIT147thGraduationTopic.EFModels
{
    public partial class MerchandiseSearch
    {
        public int MerchandiseId { get; set; }
        public string MerchandiseName { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool Display { get; set; }
    }
}
