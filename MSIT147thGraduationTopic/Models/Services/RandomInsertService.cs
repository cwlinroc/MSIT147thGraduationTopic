using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Infra.ExtendMethods;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.Infra.Utility;

namespace MSIT147thGraduationTopic.Models.Services
{
    public class RandomInsertService
    {
        private GraduationTopicContext _context;
        private RandomInsertRepository _repo;
        private RandomGenerator _generator;
        public RandomInsertService(GraduationTopicContext context)
        {
            if (context == null) context = new GraduationTopicContext();
            _context = context;
            _repo = new RandomInsertRepository(context);
            _generator = new RandomGenerator();
        }

        public void AddRandomMembers(int amount = 1)
        {
            var cities = _repo.GetCitiesAndDistricts();
            var members = new List<Member>();
            for (int i = 0; i < amount; i++)
            {
                string salt = _generator.RandomSalt();
                string account = _generator.RandomEnString();
                string password = account.GetSaltedSha256(salt);
                var city = _generator.RandomFrom(cities);
                var district = _generator.RandomFrom(city.Districts);
                members.Add(new Member
                {
                    MemberName = _generator.RandomName(),
                    NickName = _generator.RandomNickName(),
                    DateOfBirth = _generator.RandomBirthDate(),
                    Gender = _generator.RandomBool(),
                    Account = account,
                    Password = password,
                    Phone = _generator.RandomPhone(),
                    City = city.CityName,
                    District = district.DistrictName,
                    Address = _generator.RandomAddressWitoutCity(),
                    Email = _generator.RandomEmail(),
                    IsActivated = true,
                    Salt = salt
                });
            }
            _repo.AddMembers(members.ToArray());
        }


        public void AddRandomMerchandiseAndSpecs(int amount = 1)
        {
            var brands = _context.Brands.ToDictionary(o => o.BrandId, o => o.BrandName);
            var categories = _context.Categories.ToDictionary(o => o.CategoryId, o => o.CategoryName);

            for (int i = 0; i < amount; ++i)
            {
                var brand = _generator.RandomFrom(brands);
                var category = _generator.RandomFrom(categories);

                var merchandise = new Merchandise
                {
                    MerchandiseName = _generator.GetMerchandiseName(category.Key - 1),
                    BrandId = brand.Key,
                    CategoryId = category.Key,
                    Description = "窩不知道",
                    Display = true
                };

                _context.Merchandises.Add(merchandise);
                _context.SaveChanges();

                int merchandiseId = merchandise.MerchandiseId;
                int specCount = _generator.RandomIntBetween(1, 4);
                string[] specName = _generator.GetSpecName(specCount);

                for (int j = 0; j < specName.Length; ++j)
                {
                    var spec = new Spec
                    {
                        SpecName = specName[j],
                        MerchandiseId = merchandiseId,
                        Price = _generator.RandomIntBetween(100, 700),
                        Amount = _generator.RandomIntBetween(20, 200),
                        DiscountPercentage = _generator.RandomChance(70) ? _generator.RandomIntBetween(30, 99) : 100,
                        DisplayOrder = _generator.RandomIntBetween(0, 100),
                        OnShelf = true
                    };
                    _context.Specs.Add(spec);
                    _context.SaveChanges();
                }
            }

        }


        public void AddRandomCart()
        {
            var memberIds = _repo.GetAllMemberID();
            var specIds = _repo.GetAllSpecID();

            _repo.DeleteAllCartItems();

            foreach (var memberId in memberIds)
            {
                int cartItemAmount = _generator.RandomIntBetween(5, 10);

                var chosedSpecIds = _generator.RandomCollectionFrom(specIds, cartItemAmount);

                foreach (int specId in chosedSpecIds)
                {
                    _repo.AddCartItem(new CartItem
                    {
                        MemberId = memberId,
                        SpecId = specId,
                        Quantity = _generator.RandomIntBetween(1, 5),
                    });
                }
            }
        }

        public void AddRandomOrders()
        {
            var members = _context.Members.ToArray();
            var specs = _context.Specs.ToArray();

            foreach (var member in members)
            {
                int orderAmount = _generator.RandomIntBetween(1, 10);

                for (int i = 0; i < orderAmount; i++)
                {
                    var order = new Order
                    {
                        MemberId = member.MemberId,
                        PaymentMethodId = _generator.RandomIntBetween(1, 3),
                        Payed = true,
                        PurchaseTime = _generator.RandomDateBetweenDays(-100, -3),
                        DeliveryCity = String.IsNullOrEmpty(member.City) ? "臺北市" : member.City,
                        DeliveryDistrict = String.IsNullOrEmpty(member.District) ? "大安區" : member.District,
                        DeliveryAddress = member.Address,
                        ContactPhoneNumber = member.Phone
                    };

                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    int itemAmount = _generator.RandomIntBetween(1, 10);
                    var boughtSpecs = _generator.RandomCollectionFrom(specs, itemAmount);
                    int totalPrice = 0;

                    foreach (var spec in boughtSpecs)
                    {
                        var orderlist = new OrderList
                        {
                            OrderId = order.OrderId,
                            SpecId = spec.SpecId,
                            Quantity = _generator.RandomIntBetween(1, 10),
                            Price = spec.Price,
                            Discount = spec.DiscountPercentage
                        };
                        _context.OrderLists.Add(orderlist);
                        totalPrice += spec.Price * spec.DiscountPercentage / 100;
                    }
                    order.PaymentAmount = totalPrice;
                    _context.SaveChanges();
                }
            }
        }


        public void AddSpecTags()
        {
            var specIds = _repo.GetAllSpecID();
            var tagIds = _repo.GetAllTagID();
            foreach (var specId in specIds)
            {
                int tagId = _generator.RandomFrom(tagIds);
                _repo.AddSpecTags(specId, tagId);
            }
        }

        public void AddSpecPopularity()
        {
            var specIds = _repo.GetAllSpecID();
            foreach (var specId in specIds)
            {
                double popularity = _generator.RandomDouble();
                _repo.UpdateSpecPopularity(specId, popularity);
            }
        }

        //public void AddRandomEvaluations()
        //{
        //    var orders = _repo.GetAllOrdersWithMerchandiseIdAndName();

        //    foreach (var order in orders) foreach (var merchandise in order.merchandise)
        //        {
        //            if (_repo.CheckEvaluated(order.orderId, merchandise.merchandiseId)) continue;
        //            if (_generator.RandomChance(60)) continue;
        //            int score = _generator.RandomIntByWeight(0, 1, 1, 2, 10, 10);
        //            _repo.AddEvaluation(order.orderId, merchandise.merchandiseId, score);
        //        }
        //}




    }
}
