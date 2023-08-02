using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.ViewModels;

namespace MSIT147thGraduationTopic.Models.Dtos
{
    public class ShoppingHistoryDto
    {
        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public DateTime PurchaseTime { get; set; }
        public int? PaymentAmount { get; set; }
        public List<MerchandisesInOrder>? listofMerchandises { get; set; }

    }

    public class MerchandisesInOrder
    {
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }

        //商品名 
    }

    public static class ShoppingHistoryTransfer
    {
        public static ShoppingHistoryDto ToDto(this OrderWithMember entity)
        {
            return new ShoppingHistoryDto
            {
                OrderId = entity.OrderId,
                MemberId = entity.MemberId,
                PaymentMethodName = entity.PaymentMethodName,
                PurchaseTime = entity.PurchaseTime,
                PaymentAmount = entity.PaymentAmount,

            };

        }
    }
}
