using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Models.Dtos
{
    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public int MemberId { get; set; }
        public int SpecId { get; set; }
        public int Quantity { get; set; }
    }

    static public class CartItemTransfer
    {
        static public CartItemDto ToDto(this CartItem entity)
        {
            return new CartItemDto
            {
                CartItemId = entity.CartItemId,
                MemberId = entity.MemberId,
                SpecId = entity.SpecId,
                Quantity = entity.Quantity,
            };
        }
    }

}
