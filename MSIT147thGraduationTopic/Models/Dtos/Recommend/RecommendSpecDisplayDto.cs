namespace MSIT147thGraduationTopic.Models.Dtos.Recommend
{
    public class RecommendSpecDisplayDto
    {
        public int SpecId { get; set; }
        public int MerchandiseId { get; set; }
        public string? MerchandiseName { get; set; }
        public string? SpecImageName { get; set; }
        public string? MerchandiseImageName { get; set; }
        public int Price { get; set; }
        public int DiscountPercentage { get; set; }
        public List<string>? Tags { get; set; }
        public double EvaluationScore { get; set; }

    }
}
