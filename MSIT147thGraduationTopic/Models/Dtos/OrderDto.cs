using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Models.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public int PaymentMethodId { get; set; }
        public bool Payed { get; set; }
        public DateTime PurchaseTime { get; set; }
        public int? UsedCouponId { get; set; }
        public int? PaymentAmount { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryDistrict { get; set; }
        public string DeliveryAddress { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string Remark { get; set; }
    }


    static public class OrderTransfer
    {
        static public Order ToEF(this OrderDto dto)
        {
            return new Order
            {
                OrderId = dto.OrderId,
                MemberId = dto.MemberId,
                PaymentMethodId = dto.PaymentMethodId,
                Payed = dto.Payed,
                PurchaseTime = dto.PurchaseTime,
                UsedCouponId = dto.UsedCouponId,
                PaymentAmount = dto.PaymentAmount,
                DeliveryCity = dto.DeliveryCity,
                DeliveryDistrict = dto.DeliveryDistrict,
                DeliveryAddress = dto.DeliveryAddress,
                ContactPhoneNumber = dto.ContactPhoneNumber,
                Remark = dto.Remark,
            };
        }
    }
   
}
