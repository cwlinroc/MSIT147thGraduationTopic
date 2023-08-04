using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
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

        public int UpdateSpecPopularity(int specId, double popularity)
        {
            var spec = _context.Specs.FirstOrDefault(o => o.SpecId == specId);
            if (spec == null) return -1;
            spec.Popularity = popularity;
            _context.SaveChanges();
            return spec.SpecId;
        }



        public IEnumerable<(int orderId, int[] merchandiseId)> GetAllOrdersWithMerchandiseId()
        {
            var result = (from order in _context.Orders
                          join orderlist in _context.OrderLists on order.OrderId equals orderlist.OrderId
                          join spec in _context.Specs on orderlist.SpecId equals spec.SpecId
                          select new { order.OrderId, spec.MerchandiseId }).Distinct().ToList();
            return result.GroupBy(o => o.OrderId)
                .Select(o => (o.First().OrderId, o.Select(x => x.MerchandiseId).ToArray()));
        }

        public bool CheckEvaluated(int orderId, int merchandiseId)
        {
            return _context.Evaluations
                .Any(o => o.OrderId == orderId && o.MerchandiseId == merchandiseId);
        }

        public int AddEvaluation(int orderId, int merchandiseId, int score)
        {
            var evaluation = new Evaluation
            {
                MerchandiseId = merchandiseId,
                Score = score,
                OrderId = orderId
            };

            _context.Evaluations.Add(evaluation);
            _context.SaveChanges();
            return evaluation.EvaluationId;
        }

        public List<CityStructDto> GetCitiesAndDistricts()
        {
            var cities = _context.Cities.Select(o => o.ToDto()).ToList();
            foreach (var city in cities)
            {
                city.Districts = _context.Districts.Where(o => o.CityId == city.CityId)
                    .Select(o => o.ToDto()).ToList();
            }
            return cities;
        }



    }
}
