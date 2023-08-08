namespace MSIT147thGraduationTopic.Models.Dtos.Recommend
{
    public class RecommendCalculateBo
    {
        public Action<IEnumerable<RecommendationSpecsDto>>? RateEvaluation { get; set; }
        public Action<IEnumerable<RecommendationSpecsDto>>? RatePurchased { get; set; }
        public int EvaluationWeight { get; set; }
        public int PurchasedWeight { get; set; }
        public int CustomWeight { get; set; }
        public Action<IEnumerable<RecommendationSpecsDto>>? CalculatePopularity { get; set; }
    }
}
