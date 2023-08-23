using MathNet.Numerics.Random;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Infra.ExtendMethods;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.Infra.Utility;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Security.Cryptography;

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

        public async Task AddRandomOrders(int monthsBefore = 12)
        {
            var allMembers = await _context.Members.ToListAsync();

            var allSpecs = _generator.OrderByRandom(_repo.GetAllSpecs().ToList());

            int allMembersCount = allMembers.Count;
            int allSpecsCount = allSpecs.Count;

            for (int i = 0; i < monthsBefore; i++)
            {
                //未來兩天
                int minDaysBefore = (monthsBefore - i - 1) * 30 - 2;
                int maxDaysBefore = minDaysBefore + 30;
                int memberPeriod = (allMembersCount / (monthsBefore + 8)) * (i + 8);
                int specPeriod = (allSpecsCount / (monthsBefore + 8)) * (i + 8);

                await GenerateOrders(
                    allMembers.Take(memberPeriod).ToList(),
                    allSpecs.Take(specPeriod),
                    maxDaysBefore,
                    minDaysBefore);
            }


        }

        private async Task GenerateOrders(
            List<Member> members,
            IEnumerable<RandomInsertedSpecDto>? specs,
            int maxDaysBefore,
            int minDaysBefore
            )
        {
            string salt = maxDaysBefore.ToString().GetSaltedSha256();
            foreach (var member in members)
            {
                int orderAmount = (int)(_generator.RandomDouble().InvCSND(0.3) * 5);
                int paymentMethod = member.MemberName.GetHashedInt() % 3 + 1;

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
                    await _context.SaveChangesAsync();
                    await Console.Out.WriteLineAsync(order.OrderId.ToString() + " " + order.PurchaseTime.ToString());

                    int maxItemAmount = (int)(_generator.RandomDouble().InvCSND(0.2, 0.2) * 30);
                    maxItemAmount = Math.Max(maxItemAmount, 1);
                    var boughtSpecs = GetBoughtSpecs(specs!, maxItemAmount, salt);

                    int totalPrice = 0;

                    foreach (var spec in boughtSpecs)
                    {
                        int quantity = (int)((spec.FullName!
                            .GetHashedInt() / 100 % 100 / 100.0).InvCSND(0.1, 0.2) * 20);
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
                    await _context.SaveChangesAsync();
                }
            }
        }


        private List<RandomInsertedSpecDto> GetBoughtSpecs(IEnumerable<RandomInsertedSpecDto> specs, int maxItemAmount, string salt)
        {
            var boughtSpecs = _generator.RandomCollectionFrom(specs, maxItemAmount).ToList();

            foreach (var spec in boughtSpecs.ToList())
            {
                var buyChance = (((spec.FullName! + salt).GetHashedInt() % 99 + 1) / 100.0).InvCSND(0.5, 0.15);
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
                    if (_generator.RandomChance(85)) continue;
                    //int favor = 
                    int hasedInt = spec.specName.GetHashedInt();
                    int favor = (hasedInt % 100 + hasedInt / 100 % 100 + hasedInt / 10000 % 100) / 3 + 1;

                    int score = _generator.RandomIntByWeight(0,
                        (100 - favor) * (100 - favor) / 200,  // 0   50
                        (50 - favor / 2) / 5,  //0  10
                        (25 - favor / 4) / 5,  //  0  5
                        favor * 30 / 100 + 20,  // 50   20
                        favor * (favor + 10) / 100 + 20);  // 130 20
                    //string? comment = _generator.RandomChance(70) ? null : GetComment(score);
                    string? comment = GetComment(score);

                    _repo.AddEvaluation(order.orderId, spec.specId, spec.merchandiseId, score, comment);
                }
        }


        private string GetComment(int score)
        {
            return _comments[score - 1][_generator.RandomIntBetween(0, _comments[score - 1].Length - 1)];
        }

        private readonly string[][] _comments =
            {
                new string[]{ "它的描述聽起來相當吸引人。然而，收到商品後我立即感到失望。", "包裝外觀看起來還不錯，但打開後感覺到這個產品的質量很差。", "不建議購買。", "根本無法達到預期的效果", "購物送達後令人感到失望", "浪費了我的錢，不推薦給任何人。", "我真的對這種購物體驗感到非常失望。", "實際上使用起來非常麻煩。品質也很差", "沒有達到我的期望。", "這次的購買對我來說完全是一場災難。", "選購前一定要注意評價和評論的重要性。"},
                new string[]{ "整體表現還算可以，但我認為還有些地方可以改", "有些優點，但也存在改進的空間。", "總之，不值這個價錢。", "沒有達到預期的效果", "CP值不高", "超市更便宜", "質量卻差強人意，我覺得不值這個價格", "質量遠遠不如我預期的那麼好"},
                new string[]{ "不如我預期的那麼好，但這價格可以了", "整體而言還算堪用", "總體來說，這是一個中規中矩的選擇。", "商品還可以，但應該不會再回購", "大致上表現不錯", "如果可以再更便宜就好了", "是一款可以考慮的選擇，但還有些改進的空間。", "表現得還可以，但還有一些小問題需要解決。", "整體來說,毛孩可以接受", "大量購買的話可能不會考慮", "實際送來的商品讓我有些失望", "如果有其他選擇的話我應該會多方考慮"},
                new string[]{ "總的來說，這是一個不錯的產品，如果折扣再多就更好了", "還不錯ㄟ!比預期中的好很多~", "依這樣價格買到這商品，我覺得很不錯了", "下一波特價的時候會再回購", "表現可圈可點~滿意!", "這次的購物體驗很不錯，之後需要會再來", "如果可以再更多選擇就好了", "使用時需要謹慎，確保寵物的舒適和幸福。", "商品比別家便宜，但希望包裝再細心一點", "這次等待時間較長，希望流程上注意，但商品是好的", "實體跟照片有點差距", "毛寶感覺蠻喜歡", "優惠劵折扣划算", "買了很多，但品質偶爾會不在標準內", "整體還可以啦", "商品常常缺貨，但也可能是太便宜了~快點補貨啦"},
                new string[]{ "出乎意料的好!!接下來一定會成為你們的VIP", "天啊~~這價格超划算，馬上再度下單", "搶到賺到，還好我有追蹤你們網站", "包裝很用心，非常愉快的購物經驗", "現在想要買東西都會優先打開你們網頁，超級滿意!", "超快到貨~下次一定還來買", "對這次服務的經驗感到超級滿意", "服務的靈活性和可信賴性也讓我非常滿意", "非常幸運能夠找到這樣一個優質的購物網，我絕對會推薦給其他寵物愛好者", "不多說，真心推薦大家買", "良心店家，目前家裡用的都跟你們買", "絕對是我目前買過最好的商品", "體驗十分良好，必須推薦", "一不小心買滿購物車，但也太便宜了吧", "這CP值不得不推", "怎麼只有五星，我想給十顆星星阿", "這好東西不能只有我看到，馬上轉推給朋友", "太誇張了，這價格也太佛", "我家寵物真的不能沒有你們，好賣家", "優惠劵很實用，我需要多幾張!!太好買了", "看到一堆都想買，大推", "每一次買起來都好划算" },
            };

        public async Task AddSpecifySpecOrders(bool incremment, int specId, int times)
        {
            //未來兩天
            DateTime nowDate = DateTime.Now.AddDays(2);
            int[] boughtTrend = incremment ?
                new int[] { 1, 1, 2, 2, 1, 2, 3, 3, 4, 5, 20, 45 }
                : new int[] { 7, 13, 18, 20, 17, 16, 8, 7, 3, 2, 1, 0 };
            var spec = await _context.Specs.FindAsync(specId);
            for (int i = 0; i < 10; i++)
            {
                int minDaysBefore = (10 - i - 1) * 30;
                DateTime maxDate = nowDate.AddDays(-minDaysBefore);
                int maxDaysBefore = minDaysBefore + 30;
                DateTime minDate = nowDate.AddDays(-maxDaysBefore);
                int amount = boughtTrend[i] * times;
                var orderIds = await _context.Orders.Where(o => o.PurchaseTime > minDate && o.PurchaseTime < maxDate)
                    .OrderBy(o => Guid.NewGuid()).Take(amount).Select(o => o.OrderId).ToListAsync();
                orderIds = orderIds.Where(o => _generator.RandomBool()).ToList();
                var orderlists = orderIds.Select(o => new OrderList
                {
                    OrderId = o,
                    Price = spec!.Price,
                    Discount = spec.DiscountPercentage,
                    SpecId = spec.SpecId,
                    Quantity = _generator.RandomIntBetween(1, 5)
                });
                _context.OrderLists.AddRange(orderlists);
                await _context.SaveChangesAsync();
            }
        }

    }
}
