using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Models.Dtos
{
    public class SpecDto
    {
        public int SpecId { get; set; }
        public string? SpecName { get; set; }
        public int MerchandiseId { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public int DiscountPercentage { get; set; }
        public int DisplayOrder { get; set; }
        public bool OnShelf { get; set; }
    }

    static public class SpecDtoTransfer
    {
        static public SpecDto ToDto(this Spec entity)
        {
            return new SpecDto
            {
                SpecId = entity.SpecId,
                SpecName = entity.SpecName,
                MerchandiseId = entity.MerchandiseId,
                Price = entity.Price,
                Amount = entity.Amount,
                DiscountPercentage = entity.DiscountPercentage,
                DisplayOrder = entity.DisplayOrder,
                OnShelf = entity.OnShelf
            };
        }
    }
}
