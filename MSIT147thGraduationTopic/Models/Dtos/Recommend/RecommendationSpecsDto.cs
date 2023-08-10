﻿namespace MSIT147thGraduationTopic.Models.Dtos.Recommend
{
    public class RecommendationSpecsDto
    {
        public int SpecId { get; set; }
        public int MerchandiseId { get; set; }
        public double? AverageScore { get; set; }
        public int EvaluateCount { get; set; }
        public double? EvaluationRating { get; set; }
        public int PurchasedAmount { get; set; }
        public double? PurchasedRating { get; set; }
        public double? CustomRating { get; set; }
        public double? Popularity { get; set; }

    }
}
