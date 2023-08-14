namespace MSIT147thGraduationTopic.Models.Dtos.Recommend
{
    public class SpecDisplyDto
    {
        public int SpecId { get; set; }
        public int MerchandiseId { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public int DiscountPercentage { get; set; }
        public double Score { get; set; }
        public string? SpecImageName { get; set; }
        public string? MerchandiseImageName { get; set; }
    }
}
