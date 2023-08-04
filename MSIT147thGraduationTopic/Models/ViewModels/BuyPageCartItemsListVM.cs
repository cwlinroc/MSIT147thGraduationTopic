using MSIT147thGraduationTopic.Models.Dtos;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class BuyPageCartItemsListVM
    {
        public IEnumerable<CartItemDisplayDto>? CartItems { get; set; }
        public int? CouponType { get; set; }
        public int? CouponDiscountAmount { get; set; }
    }
}
