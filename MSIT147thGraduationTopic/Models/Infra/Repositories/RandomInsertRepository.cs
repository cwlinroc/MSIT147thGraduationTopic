using Dapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        public IEnumerable<RandomInsertedSpecDto> GetAllSpecs()
        {
            return _context.Specs.Select(o => new RandomInsertedSpecDto
            {
                FullName = o.Merchandise.MerchandiseName + o.SpecName,
                SpecId = o.SpecId,
                SpecName = o.SpecName,
                MerchandiseId = o.MerchandiseId,
                Price = o.Price,
                DiscountPercentage = o.DiscountPercentage,
                OnShelf = o.OnShelf
            });
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


        public int AddSpecTags(int specId, params int[] tagIds)
        {
            using var conn = new SqlConnection(_context.Database.GetConnectionString());
            foreach (var tagId in tagIds)
            {
                string str = "INSERT INTO SpecTags (SpecId,TagId) VALUES (@SpecId,@TagId)";
                conn.Execute(str, new { SpecId = specId, TagId = tagId });
            }
            return specId;
        }

        public int UpdateSpecPopularity(int specId, double popularity)
        {
            var spec = _context.Specs.FirstOrDefault(o => o.SpecId == specId);
            if (spec == null) return -1;
            spec.Popularity = popularity;
            _context.SaveChanges();
            return spec.SpecId;
        }

        public IEnumerable<(int orderId, List<(int specId, int merchandiseId, string specName)> specs)> GetAllOrdersWithSpecIdAndName()
        {
            var specs = (from order in _context.Orders
                         join orderlist in _context.OrderLists on order.OrderId equals orderlist.OrderId
                         join spec in _context.Specs on orderlist.SpecId equals spec.SpecId
                         join merchandise in _context.Merchandises on spec.MerchandiseId equals merchandise.MerchandiseId
                         select new { order.OrderId, spec.SpecId, spec.MerchandiseId, specName = merchandise.MerchandiseName + spec.SpecName })
                         .Distinct().ToList();

            return specs.GroupBy(o => o.OrderId)
                .Select(o => (o.First().OrderId, o.Select(x => (x.SpecId, x.MerchandiseId, x.specName)).ToList()));
        }

        //public IEnumerable<(int orderId, List<(int merchandiseId, string merchandiseName)> merchandise)> GetAllOrdersWithMerchandiseIdAndName()
        //{
        //    var result = (from order in _context.Orders
        //                  join orderlist in _context.OrderLists on order.OrderId equals orderlist.OrderId
        //                  join spec in _context.Specs on orderlist.SpecId equals spec.SpecId
        //                  join merchandise in _context.Merchandises on spec.MerchandiseId equals merchandise.MerchandiseId
        //                  select new { order.OrderId, spec.MerchandiseId, merchandise.MerchandiseName }).Distinct().ToList();
        //    return result.GroupBy(o => o.OrderId)
        //        .Select(o => (o.First().OrderId, o.Select(x => (x.MerchandiseId, x.MerchandiseName)).ToList()));
        //}

        public bool CheckEvaluated(int orderId, int merchandiseId)
        {
            return _context.Evaluations
                .Any(o => o.OrderId == orderId && o.MerchandiseId == merchandiseId);
        }

        public int AddEvaluation(int orderId, int specId, int merchandiseId, int score, string? comment)
        {
            var evaluation = new Evaluation
            {
                SpecId = specId,
                MerchandiseId = merchandiseId,
                Score = score,
                OrderId = orderId,
                Comment = comment
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
