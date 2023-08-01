using MSIT147thGraduationTopic.EFModels;
using System.ComponentModel;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class EvaluationVM
    {
        public int EvaluationId { get; set; }
        public int MerchandiseId { get; set; }
        //public int MemberId { get; set; }
        //public string Avatar { get; set; }
        //public string NickName { get; set; }
        [DisplayName("訂單編號")]
        public int OrderId { get; set; }
        [DisplayName("商品名稱")]
        public string MerchandiseName { get; set; }
        public int SpecId { get; set; }
        public string SpecName { get; set; }
        [DisplayName("留言")]
        public string Comment { get; set; }
        [DisplayName("星星評分")]
        public int Score { get; set; }
        public List<Comments> comments { get; set; }
        public string Keyword { get; set; }

        public class Comments //要回傳的資料
        {
            public int MerchandiseId { get; set; }
            public string MerchandiseName { get; set; }
            public int SpecId { get; set; }
            public string SpecName { get; set; }
            public string Comment { get; set; }
            public int Score { get; set; }
        }


        public virtual Member Member { get; set; }
        
        public virtual Merchandise Merchandise { get; set; }

        public virtual Order Order { get; set; }

    }
    

    static public class EvaluationTransfer
    {
        //static public EvaluationVM ToVM(this Evaluation evaluation)
        //{
        //    return new EvaluationVM
        //    {
        //        Avatar = evaluation.Avatar,
        //        NickName = evaluation.NickName,
        //        OrderId = evaluation.OrderId,
        //        MerchandiseName = evaluation.MerchandiseName,
        //        SpecName = evaluation.SpecName,
        //        Comment = evaluation.Comment,
        //        Score = evaluation.Score,
        //    };
        //}
    }
}
