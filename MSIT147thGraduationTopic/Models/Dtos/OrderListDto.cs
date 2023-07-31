using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Models.Dtos
{
    public class OrderListDto
    {
        public int OrderListId { get; set; }
        public int OrderId { get; set; }
        public int SpecId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
    }

    static public class OrderListTransfer
    {
        static public OrderList ToEF(this OrderListDto dto)
        {
            return new OrderList
            {
                OrderListId = dto.OrderListId,
                OrderId = dto.OrderId,
                SpecId = dto.SpecId,
                Quantity = dto.Quantity,
                Price = dto.Price,
                Discount = dto.Discount,
            };
        }
    }

}
