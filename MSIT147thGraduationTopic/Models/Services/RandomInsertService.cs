using MSIT147thGraduationTopic.EFModels;
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
                members.Add(new Member
                {
                    MemberName = _generator.RandomName(),
                    NickName = _generator.RandomNickName(),
                    DateOfBirth = _generator.RandomBirthDate(),
                    Gender = _generator.RandomBool(),
                    Account = _generator.RandomEnString(),
                    Password = _generator.RandomEnString(),
                    Phone = _generator.RandomPhone(),
                    Address = _generator.RandomAddress(),
                    Email = _generator.RandomEmail(),
                });
            }
            _repo.AddMembers(members.ToArray());
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


    }
}
