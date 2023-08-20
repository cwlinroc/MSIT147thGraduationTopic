using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Models.Dtos.Recommend
{
    public class RatingDataDto
    {
        public int EvaluationWeight { get; set; }
        public int PurchasedWeight { get; set; }
        public int ManuallyWeight { get; set; }
        public int RateEvaluationFunc { get; set; }
        public int RatePurchaseFunc { get; set; }
        public int? RecentEvaluationDays { get; set; }
        public int? RecentEvaluationTimes { get; set; }
        public int? RecentPurchasedDays { get; set; }
        public int? RecentPurchasedTimes { get; set; }
    }

    static public class RattingDataTransfer
    {
        static public RatingDataDto ToDto(this RatingData entity)
        {
            return new RatingDataDto
            {
                EvaluationWeight = entity.EvaluationWeight,
                PurchasedWeight = entity.PurchasedWeight,
                ManuallyWeight = entity.ManuallyWeight,
                RateEvaluationFunc = entity.RateEvaluationFunc,
                RatePurchaseFunc = entity.RatePurchaseFunc,
                RecentEvaluationDays = entity.RecentEvaluationDays,
                RecentEvaluationTimes = entity.RecentEvaluationTimes,
                RecentPurchasedDays = entity.RecentPurchasedDays,
                RecentPurchasedTimes = entity.RecentPurchasedTimes,
                
            };
        }
    }

}
