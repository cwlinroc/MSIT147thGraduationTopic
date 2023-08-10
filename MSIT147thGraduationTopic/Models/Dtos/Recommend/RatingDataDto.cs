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
            };
        }
    }

}
