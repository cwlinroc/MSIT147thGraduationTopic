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
        public List<SpecsInOrderDto>? ListOfSpecs { get; set; }

    }

    public class SpecsInOrderDto
    {
        public int MerchandiseId { get; set; }
        public string MerchandiseName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
    }

    public static class ShoppingHistoryTransfer
    {
        public static ShoppingHistoryDto ToShDto(this OrderWithMember entity)
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

        public static SpecsInOrderDto ToSpecsInOrderDto(this SpecsInOrder entity)
        {
            return new SpecsInOrderDto
            {
                MerchandiseId = entity.MerchandiseId,
                MerchandiseName = entity.MerchandiseName + entity.SpecName,
                Quantity = entity.Quantity,
                Price = entity.Price,
                Discount = entity.Discount,
            };
        }
    }
}
