using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos.Statistic;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class StatisticRepository
    {

        private readonly GraduationTopicContext _context;

        public StatisticRepository(GraduationTopicContext context)
        {
            _context = context;
        }

        public async Task<SaleChartDto?> GetSaleChart(string measurement, string classification, DateTime timeBefore)
        {
            var sum = classification switch
            {
                "quantity" => "ol.Quantity",
                "profit" => "ol.Quantity * ol.Price * ol.Discount / 100",
                _ => string.Empty
            };

            var sql = measurement switch
            {
                "category" => $@" 
SELECT c.CategoryName AS Label , SUM({sum}) as Data
FROM Categories c
JOIN Merchandises m on c.CategoryId = m.CategoryID
JOIN Specs s on m.MerchandiseID = s.MerchandiseId
JOIN OrderLists ol on s.SpecId = ol.SpecId
JOIN Orders o on ol.OrderId = ol.OrderId
WHERE o.PurchaseTime > @TimeBefore
GROUP BY c.CategoryName
",
                "animal" => $@"
SELECT t.TagName AS Label , SUM({sum}) as Data
FROM Tags t
JOIN SpecTags st ON t.TagId = st.TagId
JOIN Specs s ON st.SpecId = s.SpecId
JOIN OrderLists ol ON s.SpecId = ol.SpecId
JOIN Orders o ON ol.OrderId = ol.OrderId
WHERE t.TagId <= 4
AND o.PurchaseTime > @TimeBefore
GROUP BY t.TagName
",
                _ => string.Empty
            };

            if (sum.IsNullOrEmpty() || sql.IsNullOrEmpty()) return null;

            var conn = _context.Database.GetDbConnection();
            var result = await conn.QueryAsync<(string Label, long Data)>(sql, new { TimeBefore = timeBefore });
            return new SaleChartDto { Data = result.Select(x => x.Data).ToArray(), Labels = result.Select(o => o.Label).ToArray() };
        }


    }
}
