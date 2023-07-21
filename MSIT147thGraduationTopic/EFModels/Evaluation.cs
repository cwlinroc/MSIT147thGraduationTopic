﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MSIT147thGraduationTopic.EFModels
{
    public partial class Evaluation
    {
        public int EvaluationId { get; set; }
        public int MerchandiseId { get; set; }
        public int MemberId { get; set; }
        [DisplayName("訂單編號")]
        public int OrderId { get; set; }
        [DisplayName("留言")]
        public string Comment { get; set; }
        [DisplayName("商品名稱")]
        public string MerchandiseName { get; set; }
        public string ImageUrl { get; set; }
        [DisplayName("星星評分")]
        public int Score { get; set; }

        public virtual Member Member { get; set; }
        public virtual Merchandise Merchandise { get; set; }
        public virtual Order Order { get; set; }
    }
}