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
WITH  
r1(a,b) AS  
(
SELECT c.CategoryName AS Label , SUM({sum}) as Data
FROM Categories c
JOIN Merchandises m on c.CategoryId = m.CategoryID
JOIN Specs s on m.MerchandiseID = s.MerchandiseId
JOIN OrderLists ol on s.SpecId = ol.SpecId
JOIN Orders o on ol.OrderId = ol.OrderId
WHERE o.PurchaseTime > @TimeBefore
GROUP BY c.CategoryName
),
r2(a) AS 
(
SELECT c.CategoryName  
FROM Categories c 
)
SELECT r2.a AS Label, COALESCE( b, 0) AS Data FROM r2
LEFT JOIN r1 ON r1.a = r2.a
",
                "animal" => $@"
WITH  
r1(a,b) AS  
(
SELECT t.TagName AS Label , SUM({sum}) as Data
FROM Tags t
JOIN SpecTags st ON t.TagId = st.TagId
JOIN Specs s ON st.SpecId = s.SpecId
JOIN OrderLists ol ON s.SpecId = ol.SpecId
JOIN Orders o ON ol.OrderId = ol.OrderId
WHERE t.TagId <= 4
AND o.PurchaseTime > @TimeBefore
GROUP BY t.TagName
),
r2(a) AS 
(
SELECT t.TagName AS Label 
FROM Tags t
WHERE t.TagId <= 4
)
SELECT r2.a AS Label, COALESCE( b, 0) AS Data FROM r2
LEFT JOIN r1 ON r1.a = r2.a
",
                "brand" => $@"
WITH  
r1(a,b) AS  
(
SELECT b.BrandName AS Label , SUM({sum}) as Data
FROM Brands b
JOIN Merchandises m ON m.BrandID = b.BrandId
JOIN Specs s ON m.MerchandiseID = s.MerchandiseID
JOIN OrderLists ol ON s.SpecId = ol.SpecId
JOIN Orders o ON ol.OrderId = ol.OrderId
WHERE o.PurchaseTime > @TimeBefore
GROUP BY b.BrandName
),
r2(a) AS 
(
SELECT b.BrandName 
FROM Brands b
)
SELECT r2.a AS Label, COALESCE( b, 0) AS Data FROM r2
LEFT JOIN r1 ON r1.a = r2.a
",
                _ => string.Empty
            };

            if (sum.IsNullOrEmpty() || sql.IsNullOrEmpty()) return null;

            var conn = _context.Database.GetDbConnection();
            var result = await conn.QueryAsync<(string Label, long Data)>(sql, new { TimeBefore = timeBefore });
            return new SaleChartDto { Data = result.Select(x => x.Data).ToArray(), Labels = result.Select(o => o.Label).ToArray() };
        }


        public async Task<IEnumerable<(string, long)>?> GetSalesTrendPeriod(
            string measurement, 
            string classification, 
            DateTime startDate, 
            DateTime endDate)
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
WITH  
r1(a,b) AS  
(
SELECT c.CategoryName AS Label , SUM({sum}) AS Data 
FROM Categories c
JOIN Merchandises m on c.CategoryId = m.CategoryID
JOIN Specs s on m.MerchandiseID = s.MerchandiseId
JOIN OrderLists ol on s.SpecId = ol.SpecId
JOIN Orders o on ol.OrderId = ol.OrderId
WHERE o.PurchaseTime BETWEEN @StartTime AND @EndTime
GROUP BY c.CategoryName
),
r2(a) AS 
(
SELECT c.CategoryName  
FROM Categories c 
)
SELECT r2.a AS Label, COALESCE( b, 0) AS Data FROM r2
LEFT JOIN r1 ON r1.a = r2.a
",
                "animal" => $@"
WITH  
r1(a,b) AS  
(
SELECT t.TagName AS Label , SUM({sum}) as Data
FROM Tags t
JOIN SpecTags st ON t.TagId = st.TagId
JOIN Specs s ON st.SpecId = s.SpecId
JOIN OrderLists ol ON s.SpecId = ol.SpecId
JOIN Orders o ON ol.OrderId = ol.OrderId
WHERE t.TagId <= 4
AND o.PurchaseTime BETWEEN @StartTime AND @EndTime
GROUP BY t.TagName
),
r2(a) AS 
(
SELECT t.TagName AS Label 
FROM Tags t
WHERE t.TagId <= 4
)
SELECT r2.a AS Label, COALESCE( b, 0) AS Data FROM r2
LEFT JOIN r1 ON r1.a = r2.a
",
                "brand" => $@"
WITH  
r1(a,b) AS  
(
SELECT b.BrandName AS Label , SUM({sum}) as Data
FROM Brands b
JOIN Merchandises m ON m.BrandID = b.BrandId
JOIN Specs s ON m.MerchandiseID = s.MerchandiseID
JOIN OrderLists ol ON s.SpecId = ol.SpecId
JOIN Orders o ON ol.OrderId = ol.OrderId
WHERE o.PurchaseTime BETWEEN @StartTime AND @EndTime
GROUP BY b.BrandName
),
r2(a) AS 
(
SELECT b.BrandName 
FROM Brands b
)
SELECT r2.a AS Label, COALESCE( b, 0) AS Data FROM r2
LEFT JOIN r1 ON r1.a = r2.a
",
                _ => string.Empty
            };
            if (sum.IsNullOrEmpty() || sql.IsNullOrEmpty()) return null;

            var conn = _context.Database.GetDbConnection();
            var result =  await conn.QueryAsync<(string Label, long Data)>(sql, new { StartTime = startDate, EndTime = endDate });
            return result;
        }

    }
}
