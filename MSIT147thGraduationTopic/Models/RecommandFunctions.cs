using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos.Recommend;

namespace MSIT147thGraduationTopic.Models
{
    public class RecommandFunctions
    {
        static public Action<IEnumerable<RecommendationSpecsDto>, RatingDataDto> RateEvaluationWithBayesianAverage
        {
            get => (specs, ratingData) =>
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
                //Recent
                if (!ratingData.RecentEvaluationDays.HasValue || !ratingData.RecentEvaluationTimes.HasValue || ratingData.RecentEvaluationTimes == 0) return;

                int recentEvaluationConutOfAll = specs.Sum(o => o.RecentEvaluateCount!.Value);
                double recentAverageScoreOfAll = specs.Sum(o => (o.RecentAverageScore ?? 3.0) * o.RecentEvaluateCount!.Value) / recentEvaluationConutOfAll;

                foreach (var spec in specs.ToList())
                {
                    if (spec.RecentEvaluateCount <= 0) { spec.EvaluationRating /= (1 + ratingData.RecentEvaluationTimes); continue; }
                    var recentRate = (recentEvaluationConutOfAll * recentAverageScoreOfAll + spec.RecentEvaluateCount * spec.RecentAverageScore)
                        / (recentEvaluationConutOfAll + spec.RecentEvaluateCount) / 5.0;
                    spec.EvaluationRating = (recentRate + spec.EvaluationRating)
                        / (1 + ratingData.RecentEvaluationTimes);
                }

            };
        }
        static public Action<IEnumerable<RecommendationSpecsDto>, RatingDataDto> RateEvaluationWithMathematicaMean
        {
            get => (specs, ratingData) =>
            {
                foreach (var spec in specs.ToList())
                {
                    if (spec.EvaluateCount <= 0) { spec.EvaluationRating = 0.5; continue; }
                    spec.EvaluationRating = spec.AverageScore / 5.0;
                }
                //Recent
                if (!ratingData.RecentEvaluationDays.HasValue || !ratingData.RecentEvaluationTimes.HasValue || ratingData.RecentEvaluationTimes == 0) return;

                foreach (var spec in specs.ToList())
                {
                    if (spec.RecentEvaluateCount <= 0) { spec.EvaluationRating /= (1 + ratingData.RecentEvaluationTimes); continue; }
                    spec.EvaluationRating = ((spec.RecentAverageScore / 5.0 * ratingData.RecentEvaluationTimes) + spec.EvaluationRating)
                        / (1 + ratingData.RecentEvaluationTimes);
                }
            };
        }
        static public Action<IEnumerable<RecommendationSpecsDto>, RatingDataDto> RateEvaluationByRanking
        {
            get => (specs, ratingData) =>
            {
                int length = specs.Count();
                double period = 1.0 / length;
                var specList = specs.OrderBy(o => o.AverageScore).ToList();
                for (int i = 0; i < length; i++)
                {
                    specList[i].EvaluationRating = period * i;
                }

                //Recent
                if (!ratingData.RecentEvaluationDays.HasValue || !ratingData.RecentEvaluationTimes.HasValue || ratingData.RecentEvaluationTimes == 0) return;

                var recentSpecList = specList.OrderBy(o => o.RecentAverageScore ?? 0).ToList();
                for (int i = 0; i < length; i++)
                {
                    var recentRate = period * i;

                    recentSpecList[i].EvaluationRating = (recentRate + recentSpecList[i].EvaluationRating)
                        / (1 + ratingData.RecentEvaluationTimes);
                }

            };
        }

        static public Action<IEnumerable<RecommendationSpecsDto>, RatingDataDto> RatePurchasedWithLogTransform
        {
            get => (specs, ratingData) =>
            {
                double maxPurchasedRate = Math.Log2(specs.Max(o => o.PurchasedAmount));
                foreach (var spec in specs.ToList())
                {
                    if (spec.PurchasedAmount <= 0) { spec.PurchasedRating = 0; continue; }
                    spec.PurchasedRating = Math.Log2(spec.PurchasedAmount) / maxPurchasedRate;
                }

                //Recent
                if (!ratingData.RecentPurchasedTimes.HasValue || !ratingData.RecentPurchasedDays.HasValue || ratingData.RecentPurchasedTimes == 0) return;

                double recentMaxPurchasedRate = Math.Log2(specs.Max(o => o.RecentPurchasedAmount ?? 0));
                foreach (var spec in specs.ToList())
                {
                    if (spec.RecentPurchasedAmount <= 0) { spec.PurchasedRating /= (1 + ratingData.RecentPurchasedTimes); continue; }

                    var recentRate = Math.Log2(spec.RecentPurchasedAmount ?? 0) / recentMaxPurchasedRate;
                    spec.PurchasedRating = (recentRate + spec.PurchasedRating)
                        / (1 + ratingData.RecentPurchasedTimes);
                }

            };
        }

        static public Action<IEnumerable<RecommendationSpecsDto>, RatingDataDto> RatePurchasedWithProportion
        {
            get => (specs, ratingData) =>
            {
                double maxPurchasedRate = specs.Max(o => o.PurchasedAmount);
                foreach (var spec in specs.ToList())
                {
                    if (spec.PurchasedAmount <= 0) { spec.PurchasedRating = 0; continue; }
                    spec.PurchasedRating = spec.PurchasedAmount / maxPurchasedRate;
                }

                //Recent
                if (!ratingData.RecentPurchasedTimes.HasValue || !ratingData.RecentPurchasedDays.HasValue || ratingData.RecentPurchasedTimes == 0) return;

                double recentMaxPurchasedRate = specs.Max(o => o.RecentPurchasedAmount ?? 0);
                foreach (var spec in specs.ToList())
                {
                    if (spec.RecentPurchasedAmount <= 0) { spec.PurchasedRating /= (1 + ratingData.RecentPurchasedTimes); continue; }

                    var recentRate = (spec.RecentPurchasedAmount ?? 0) / recentMaxPurchasedRate;
                    spec.PurchasedRating = (recentRate + spec.PurchasedRating)
                        / (1 + ratingData.RecentPurchasedTimes);
                }
            };
        }

        static public Action<IEnumerable<RecommendationSpecsDto>, RatingDataDto> RatePurchasedByRanking
        {
            get => (specs, ratingData) =>
            {
                int length = specs.Count();
                double period = 1.0 / length;
                var specList = specs.OrderBy(o => o.PurchasedAmount).ToList();
                for (int i = 0; i < length; i++)
                {
                    specList[i].PurchasedRating = period * i;
                }

                //Recent
                if (!ratingData.RecentPurchasedTimes.HasValue || !ratingData.RecentPurchasedDays.HasValue || ratingData.RecentPurchasedTimes == 0) return;

                var recentSpecList = specList.OrderBy(o => o.RecentPurchasedAmount ?? 0).ToList();
                for (int i = 0; i < length; i++)
                {
                    var recentRate = period * i;

                    recentSpecList[i].PurchasedRating = (recentRate + recentSpecList[i].PurchasedRating)
                        / (1 + ratingData.RecentPurchasedTimes);
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
