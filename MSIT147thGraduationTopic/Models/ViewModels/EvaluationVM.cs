using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class EvaluationVM
    {
        public int EvaluationId { get; set; }
        public int MerchandiseId { get; set; }
        public int MemberId { get; set; }
        public int OrderId { get; set; }
        public string Comment { get; set; }
        public int Score { get; set; }

        public virtual Member Member { get; set; }
        public virtual Merchandise Merchandise { get; set; }
        public virtual Order Order { get; set; }
    }
}
