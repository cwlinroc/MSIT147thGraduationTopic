namespace MSIT147thGraduationTopic.Models.Dtos
{
    public class RandomInsertedSpecDto
    {
        public int SpecId { get; set; }
        public string? SpecName { get; set; }
        public string? FullName { get; set; }
        public int MerchandiseId { get; set; }
        public int Price { get; set; }
        public int DiscountPercentage { get; set; }
        public bool OnShelf { get; set; }
    }
    
}
