namespace MSIT147thGraduationTopic.Models.Dtos
{
    public class CartItemCheckoutDto
    {
        public int CartItemId { get; set; }
        public int SpecId { get; set; } 
        public int MerchandiseId { get; set; }
        public string ItemName { get; set; }
        public int DiscountPercentage { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int Amount { get; set; }
        public bool OnShelf { get; set; }
        public List<int>? TagIds { get; set; }
    }
}
