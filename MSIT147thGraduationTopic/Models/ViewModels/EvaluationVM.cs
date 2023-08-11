using MSIT147thGraduationTopic.EFModels;
using System.ComponentModel;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class EvaluationVM
    {
        public int EvaluationId { get; set; }
        public int MerchandiseId { get; set; }
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
        public string StarImageUrl { get; set; }
        public string Keyword { get; set; }
        public List<Comments> comments { get; set; }
        

        public class Comments //要回傳的資料
        {
            public int EvaluationId { get; set; }
            public int MerchandiseId { get; set; }
            public string MerchandiseName { get; set; }
            public int SpecId { get; set; }
            public string SpecName { get; set; }
            public string Comment { get; set; }
            public int Score { get; set; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Evaluation eother = (Evaluation)obj;
            Spec sother= (Spec)obj;
            return EvaluationId == eother.EvaluationId && MerchandiseId == eother.MerchandiseId && SpecId == sother.SpecId;
        }
        public override int GetHashCode()
        {
            return new { EvaluationId, MerchandiseId, SpecId }.GetHashCode();
        }



        public virtual Member Member { get; set; }
        
        public virtual Merchandise Merchandise { get; set; }

        public virtual Order Order { get; set; }

       

    }
    

    
}
