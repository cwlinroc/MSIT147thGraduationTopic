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
            var members = new List<Member>();
            for (int i = 0; i < amount; i++)
            {
                string salt = _generator.RandomSalt();
                string account = _generator.RandomEnString();
                string password = account.GetSaltedSha256(salt);
                members.Add(new Member
                {
                    MemberName = _generator.RandomName(),
                    NickName = _generator.RandomNickName(),
                    DateOfBirth = _generator.RandomBirthDate(),
                    Gender = _generator.RandomBool(),
                    Account = account,
                    Password = password,
                    Phone = _generator.RandomPhone(),
                    Address = _generator.RandomAddress(),
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
                int cartItemAmount = _generator.RandomIntBetween(1, 5);

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
            var memberIds = _repo.GetAllMemberID();
            var specIds = _repo.GetAllSpecID();

            foreach (var memberId in memberIds)
            {
                int orderAmount = _generator.RandomIntBetween(1, 10);

                var member = _context.Members.FirstOrDefault(m => m.MemberId == memberId);
                //var chosedSpecIds = _generator.RandomCollectionFrom(specIds, cartItemAmount);

                for (int i = 0; i < orderAmount; i++)
                {
                    var order = new Order
                    {
                        MemberId = memberId,
                        PaymentMethodId = _generator.RandomIntBetween(1, 3),
                        Payed = true,
                        PurchaseTime = _generator.RandomDateBetweenDays(-100, -3),
                        DeliveryAddress = member.Address,
                        ContactPhoneNumber = member.Phone

                    };




                }
            }
        }

        //public int OrderId { get; set; }
        //public int MemberId { get; set; }
        //public int PaymentMethodId { get; set; }
        //public bool Payed { get; set; }
        //public DateTime PurchaseTime { get; set; }
        //public int? UsedCouponId { get; set; }
        //public int? PaymentAmount { get; set; }
        //public string DeliveryAddress { get; set; }
        //public string ContactPhoneNumber { get; set; }
        //public string Remark { get; set; }


    }
}
