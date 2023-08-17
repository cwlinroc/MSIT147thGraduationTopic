using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Dtos.LinePay;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Collections.Generic;
using System.Transactions;
using static MSIT147thGraduationTopic.Controllers.Buy.ApiBuyController;

namespace MSIT147thGraduationTopic.Models.Services
{
    public class BuyServices
    {
        private readonly GraduationTopicContext _context;
        private readonly BuyRepository _repo;

        public BuyServices(GraduationTopicContext context)
        {
            _context = context;
            _repo = new BuyRepository(context);
        }

        public async Task<IEnumerable<CartItemDisplayDto>> GetCartItems(int[] cartItemIds)
        {
            return await _repo.GetCartItems(cartItemIds);
        }

        //public async Task<int> CalculatePaymentPrice(int[] cartItemIds, int couponId)
        //{
        //    var list = GetCartItemsWithCoupons(cartItemIds, couponId);

        //}


        public async Task<BuyPageCartItemsListVM?> GetCartItemsWithCoupons(int[] cartItemIds, int? couponId)
        {
            if (cartItemIds.IsNullOrEmpty()) return null;
            //var carItemsTask = _repo.GetCartItems(cartItemIds);
            //var couponTask = _repo.GetCouponById(couponId);
            //await Task.WhenAll(carItemsTask, couponTask);
            //return CalculateCouponResult(couponTask.Result, carItemsTask.Result);
            var carItems = await _repo.GetCartItems(cartItemIds);
            var coupon = await _repo.GetCouponById(couponId);

            return CalculateCouponResult(coupon, carItems.ToList());
        }

        private BuyPageCartItemsListVM CalculateCouponResult(CouponDto? coupon, List<CartItemDisplayDto> cartItems)
        {
            var cartItemsList = new BuyPageCartItemsListVM { CartItems = cartItems };
            List<CartItemDisplayDto> filteredCartItems = cartItems;

            if (coupon == null || coupon.CouponStartDate > DateTime.Now
                || coupon.CouponEndDate < DateTime.Now)
            {
                return cartItemsList;
            }
            if (coupon.CouponDiscountTypeId == 0)
            {
                cartItemsList.CouponType = 0;
                if (coupon.CouponTagId != null) filteredCartItems = cartItems
                        .Where(o => !o.Tags.IsNullOrEmpty() && o.Tags!.Contains(coupon.CouponTagId.Value)).ToList();
                foreach (var item in filteredCartItems) item.CouponDiscount = (int)coupon.CouponDiscount;
            }
            if (coupon.CouponDiscountTypeId == 1)
            {
                cartItemsList.CouponType = 1;
                if (coupon.CouponTagId != null) filteredCartItems = cartItems
                        .Where(o => !o.Tags.IsNullOrEmpty() && o.Tags!.Contains(coupon.CouponTagId.Value)).ToList();
                int sum = filteredCartItems.Sum(o => (o.CartItemPrice * o.DiscountPercentage / 100) * o.Quantity);
                if (sum >= coupon.CouponCondition) cartItemsList.CouponDiscountAmount = (int)coupon.CouponDiscount;
            }
            return cartItemsList;
        }

        public MemberDto? GetMemberData(int cartItemId)
        {
            int memberId = _repo.GetMemberIdByCartItemId(cartItemId);

            var member = _repo.GetMemberData(memberId);

            return member;
        }

        public async Task<IEnumerable<BuyPageCouponVM>> GetAllCouponsAvalible(int memberId)
        {
            return (await _repo.GetAllCouponsAvalible(memberId))
                .Select(o => new BuyPageCouponVM { CouponId = o.CouponId, CouponName = o.CouponName });
        }

        public async Task<(int, int, List<CartItemCheckoutDto>)> CreateOrder(int[] cartItemIds, int memberId, OrderRecord record)
        {
            var checkoutDto = await _repo.GetCheckoutInformation(cartItemIds);

            int? couponId = int.TryParse(record.CouponId, out int tempNum) ? tempNum : null;
            int totalPayment = await CalculateTotalPayment(checkoutDto, couponId);

            var order = new OrderDto
            {
                MemberId = memberId,
                PaymentMethodId = int.Parse(record.Payment),
                Payed = false,
                PurchaseTime = DateTime.Now,
                UsedCouponId = (record.CouponId != null) ? int.Parse(record.CouponId) : null,
                PaymentAmount = totalPayment,
                DeliveryCity = record.City,
                DeliveryDistrict = record.District,
                DeliveryAddress = record.Address,
                ContactPhoneNumber = record.Phone,
                Remark = record.Remark
            };

            //Transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            int orderId = _repo.CreateOrder(order);

            if (orderId <= 0) return (-1, -1, new());

            var orderLists = checkoutDto.Select(o =>
            {
                var dto = new OrderListDto
                {
                    OrderId = orderId,
                    SpecId = o.SpecId,
                    Quantity = o.Quantity,
                    Price = o.Price,
                    Discount = o.DiscountPercentage
                };
                return dto;
            });

            int listCreated = _repo.CreateOrderLists(orderLists);
            if (listCreated <= 0) return (-1, -1, new());

            int cartItemsDeleted = _repo.ClearCartItems(cartItemIds);
            if (cartItemsDeleted <= 0) return (-1, -1, new());

            int specStorageReduced = await _repo.ReduceSpecStorage(orderLists);
            if (specStorageReduced <= 0) return (-1, -1, new());

            if ((record.CouponId != null)) await _repo.MakeCouponUsed(memberId, order.UsedCouponId!.Value);

            await _context.Database.CommitTransactionAsync();
            return (orderId, totalPayment, checkoutDto);
        }

        private async Task<int> CalculateTotalPayment(IEnumerable<CartItemCheckoutDto> checkoutDtos, int? couponId)
        {
            CouponDto? coupon = null;
            if (couponId != null && couponId > 0) coupon = await _repo.GetCouponById(couponId);

            //無優惠卷或優惠卷無效
            if (coupon == null
                || coupon.CouponStartDate > DateTime.Now
                || coupon.CouponEndDate < DateTime.Now)
            {
                return checkoutDtos.Sum(o => (o.Price * o.DiscountPercentage / 100) * o.Quantity);
            }

            //優惠卷type 0
            if (coupon.CouponDiscountTypeId == 0 && coupon.CouponTagId == null)
            {
                return checkoutDtos.Sum(o => (o.Price * o.DiscountPercentage / 100)
                    * (int)coupon.CouponDiscount / 100 * o.Quantity);
            }
            if (coupon.CouponDiscountTypeId == 0 && coupon.CouponTagId != null)
            {
                return checkoutDtos.Sum(o =>
                {
                    int discount = 100;
                    if (!o.TagIds.IsNullOrEmpty() && o.TagIds!.Contains(coupon.CouponTagId.Value))
                    {
                        discount = (int)coupon.CouponDiscount;
                    }
                    return (o.Price * o.DiscountPercentage / 100) * discount / 100 * o.Quantity;
                });
            }


            //優惠卷type 1
            if (coupon.CouponDiscountTypeId == 1 && coupon.CouponTagId == null)
            {
                int total = checkoutDtos.Sum(o => (o.Price * o.DiscountPercentage / 100) * o.Quantity);
                return (coupon.CouponCondition < total) ? total - (int)coupon.CouponDiscount : total;
            }
            if (coupon.CouponDiscountTypeId == 1 && coupon.CouponTagId != null)
            {
                int total = checkoutDtos.Sum(o => (o.Price * o.DiscountPercentage / 100) * o.Quantity);
                int requiredTotal = checkoutDtos
                    .Where(o => !o.TagIds.IsNullOrEmpty() && o.TagIds!.Contains(coupon.CouponTagId.Value))
                    .Sum(o => (o.Price * o.DiscountPercentage / 100) * o.Quantity);
                return (coupon.CouponCondition < requiredTotal) ? total - (int)coupon.CouponDiscount : total;
            }


            //else
            return checkoutDtos.Sum(o => (o.Price * o.DiscountPercentage / 100) * o.Quantity);
        }

        public async Task<int> ConfirmOrder(int orderId)
        {
            if (orderId <= 0) return -1;
            return await _repo.ConfirmOrder(orderId);
        }

    }
}
