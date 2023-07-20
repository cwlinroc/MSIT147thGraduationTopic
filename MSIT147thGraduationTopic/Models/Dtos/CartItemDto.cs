namespace MSIT147thGraduationTopic.Models.Dtos
{
    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int SpecId { get; set; }
        public int Quantity { get; set; }
    }
}
