namespace MSIT147thGraduationTopic.Models.Dtos.Recommend
{
    public class RecommendCalculateBo
    {
        public Action<IEnumerable<RecommendationSpecsDto>, RatingDataDto>? RateEvaluationFunc { get; set; }
        public Action<IEnumerable<RecommendationSpecsDto>, RatingDataDto>? RatePurchased { get; set; }
        public int EvaluationWeight { get; set; }
        public int PurchasedWeight { get; set; }
        public int ManuallyWeight { get; set; }
    }
}
