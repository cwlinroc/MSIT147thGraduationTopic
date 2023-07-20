using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Infra.Utility;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class RandomInsertRepository
    {
        private GraduationTopicContext _context;
        public RandomInsertRepository(GraduationTopicContext context)
        {
            if (context == null) context = new GraduationTopicContext();
            _context = context;
        }

        public void AddMembers(params Member[] members)
        {
            _context.Members.AddRange(members);
            _context.SaveChanges();
        }

        public IEnumerable<int> GetAllMemberID()
        {
            var ids = _context.Members.Select(o => o.MemberId);
            return ids;
        }

        public IEnumerable<int> GetAllSpecID()
        {
            var ids = _context.Specs.Select(o => o.SpecId);
            return ids;
        }

        public int GetCartID(int memberId)
        {
            var cart = _context.Carts.FirstOrDefault(o => o.MemberId == memberId);
            return (cart == null) ? -1 : cart.CartId;
        }

        public int AddCart(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
            return cart.CartId;
        }

        public int AddCartItem(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            _context.SaveChanges();
            return cartItem.CartId;
        }


    }
}
