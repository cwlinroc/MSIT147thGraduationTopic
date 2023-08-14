using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Infra.ExtendMethods;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.Infra.Utility;

namespace MSIT147thGraduationTopic.Models.Services
{
    public class SimulationDataInsertService
    {


        private GraduationTopicContext _context;
        private RandomInsertRepository _repo;
        private RandomGenerator _generator;
        public SimulationDataInsertService(GraduationTopicContext context)
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
                // 照人口分配都市
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

        public void AddRandomOrders(int maxDaysBefore = 180, int minDaysBefore = 3)
        {
            var members = _context.Members.ToArray();
            //用隨機(?)tag取得spec
            var specs = _repo.GetAllSpecs();

            foreach (var member in members)
            {
                int orderAmount = (int)(_generator.RandomDouble().InvCSND(0.3) * 30);
                int paymentMethod = member.MemberName.GetHashedInt() % 3 + 1;
                int record = paymentMethod;

                for (int i = 0; i < orderAmount; i++)
                {
                    var order = new Order
                    {
                        MemberId = member.MemberId,
                        PaymentMethodId = paymentMethod,
                        Payed = true,
                        PurchaseTime = _generator.RandomDateBetweenDays(-maxDaysBefore, -minDaysBefore),
                        DeliveryCity = String.IsNullOrEmpty(member.City) ? "臺北市" : member.City,
                        DeliveryDistrict = String.IsNullOrEmpty(member.District) ? "大安區" : member.District,
                        DeliveryAddress = member.Address,
                        ContactPhoneNumber = member.Phone
                    };

                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    int maxItemAmount = (int)(_generator.RandomDouble().InvCSND(0.2, 0.2) * 20);
                    maxItemAmount = Math.Max(maxItemAmount, 1);
                    var boughtSpecs = GetBoughtSpecs(specs, maxItemAmount);

                    int totalPrice = 0;

                    foreach (var spec in boughtSpecs)
                    {
                        int quantity = (int)((spec.FullName!.GetHashedInt() / 100 % 100 / 100.0).InvCSND(0.1, 0.2) * 20);
                        quantity = Math.Max(quantity, 1);

                        var orderlist = new OrderList
                        {
                            OrderId = order.OrderId,
                            SpecId = spec.SpecId,
                            Quantity = quantity,
                            Price = spec.Price,
                            Discount = spec.DiscountPercentage
                        };
                        _context.OrderLists.Add(orderlist);
                        int sum = spec.Price * spec.DiscountPercentage / 100 * quantity;
                        totalPrice += sum;
                    }
                    order.PaymentAmount = totalPrice;
                    _context.SaveChanges();
                }
            }
        }


        private List<RandomInsertedSpecDto> GetBoughtSpecs(IEnumerable<RandomInsertedSpecDto> specs, int maxItemAmount)
        {
            var boughtSpecs = _generator.RandomCollectionFrom(specs, maxItemAmount).ToList();

            foreach (var spec in boughtSpecs.ToList())
            {
                var buyChance = (spec.FullName!.GetHashedInt() % 100 / 100.0).InvCSND();
                if (buyChance < _generator.RandomDouble()) boughtSpecs.Remove(spec);
            }

            if (boughtSpecs.IsNullOrEmpty()) boughtSpecs.Add(_generator.RandomFrom(specs));
            return boughtSpecs;
        }


        public void AddSpecTags()
        {
            var specIds = _repo.GetAllSpecID();
            var tagIds = _repo.GetAllTagID();
            foreach (var specId in specIds)
            {
                int[] tagIdsChoosed = _generator.RandomCollectionFrom(tagIds, _generator.RandomIntBetween(1, 3)).ToArray();
                _repo.AddSpecTags(specId, tagIdsChoosed);
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


        public void AddRandomEvaluations()
        {
            var orders = _repo.GetAllOrdersWithSpecIdAndName();

            foreach (var order in orders) foreach (var spec in order.specs)
                {
                    if (_repo.CheckEvaluated(order.orderId, spec.specId)) continue;
                    if (_generator.RandomChance(60)) continue;
                    //int favor = 
                    int hasedInt = spec.specName.GetHashedInt();
                    int favor = (hasedInt % 100 + hasedInt / 100 % 100 + hasedInt / 10000 % 100) / 3 + 1;

                    int score = _generator.RandomIntByWeight(0,
                        (100 - favor) * (100 - favor) / 200,  // 0   50
                        (50 - favor / 2) / 5,  //0  10
                        (25 - favor / 4) / 5,  //  0  5
                        favor * 30 / 100 + 20,  // 50   20
                        favor * (favor + 10) / 100 + 20);  // 130 20
                    _repo.AddEvaluation(order.orderId, spec.specId, spec.merchandiseId ,score);
                }
        }


    }
}
