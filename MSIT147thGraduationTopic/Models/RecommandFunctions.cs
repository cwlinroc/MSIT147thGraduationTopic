using MSIT147thGraduationTopic.Models.Dtos.Recommend;

namespace MSIT147thGraduationTopic.Models
{
    public class RecommandFunctions
    {
        static public Action<IEnumerable<RecommendationSpecsDto>> RateEvaluationWithBayesianAverage
        {
            get => specs =>
            {
                int evaluationConutOfAll = specs.Sum(o => o.EvaluateCount);
                double averageScoreOfAll = specs.Sum(o => (o.AverageScore ?? 3.0) * o.EvaluateCount) / evaluationConutOfAll;

                foreach (var spec in specs.ToList())
                {
                    if (spec.EvaluateCount <= 0) { spec.EvaluationRating = 0.5; continue; }
                    spec.EvaluationRating =
                        (evaluationConutOfAll * averageScoreOfAll + spec.EvaluateCount * spec.AverageScore)
                        / (evaluationConutOfAll + spec.EvaluateCount) / 5.0;
                }
            };
        }
        static public Action<IEnumerable<RecommendationSpecsDto>> RateEvaluationWithMathematicaMean
        {
            get => specs =>
            {
                foreach (var spec in specs.ToList())
                {
                    if (spec.EvaluateCount <= 0) { spec.EvaluationRating = 0.5; continue; }
                    spec.EvaluationRating = spec.AverageScore / 5.0;
                }
            };
        }

        static public Action<IEnumerable<RecommendationSpecsDto>> RatePurchasedWithLogTransform
        {
            get => specs =>
            {
                double maxPurchasedRate = Math.Log2(specs.Max(o => o.PurchasedAmount));
                foreach (var spec in specs.ToList())
                {
                    if (spec.PurchasedAmount <= 0) { spec.PurchasedRating = 0; continue; }
                    spec.PurchasedRating = Math.Log2(spec.PurchasedAmount) / maxPurchasedRate;
                }
            };
        }

        static public Action<IEnumerable<RecommendationSpecsDto>> RatePurchasedWithProportion
        {
            get => specs =>
            {
                double maxPurchasedRate = specs.Max(o => o.PurchasedAmount);
                foreach (var spec in specs.ToList())
                {
                    if (spec.PurchasedAmount <= 0) { spec.PurchasedRating = 0; continue; }
                    spec.PurchasedRating = spec.PurchasedAmount / maxPurchasedRate;
                }
            };
        }

        static public Action<IEnumerable<RecommendationSpecsDto>, int, int, int> CalculatePopularity
        {
            get => (specs, evaluateWeight, purchaseWeight, customWeight) =>
            {
                if (evaluateWeight == 0 && purchaseWeight == 0 && customWeight == 0)
                {
                    foreach (var spec in specs.ToList()) spec.Popularity = 0.5;
                    return;
                }
                foreach (var spec in specs.ToList())
                {
                    spec.Popularity = (spec.EvaluationRating * evaluateWeight + spec.PurchasedRating * purchaseWeight + spec.CustomRating * customWeight)
                        / (evaluateWeight + purchaseWeight + customWeight);
                }
            };
        }



    }
}
