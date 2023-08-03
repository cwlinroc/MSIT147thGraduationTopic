using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        public void DeleteAllCartItems()
        {
            _context.CartItems.RemoveRange(_context.CartItems);
            _context.SaveChanges();
        }


        public int AddCartItem(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            _context.SaveChanges();
            return cartItem.MemberId;
        }

        public IEnumerable<int> GetAllTagID()
        {
            var ids = _context.Tags.Select(o => o.TagId); 
            return ids;
        }

        
        public int AddSpecTags(int specId, int tagId)
        {
            using var conn = new SqlConnection(_context.Database.GetConnectionString());
            string str = "INSERT INTO SpecTags (SpecId,TagId) VALUES (@SpecId,@TagId)";
            return conn.Execute(str, new { SpecId = specId, TagId = tagId });
        }


    }
}
