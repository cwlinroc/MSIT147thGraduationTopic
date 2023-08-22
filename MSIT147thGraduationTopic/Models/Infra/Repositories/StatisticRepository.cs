using Dapper;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos.Statistic;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Data;
using System.Text.RegularExpressions;

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
JOIN Orders o on ol.OrderId = o.OrderId
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
JOIN Orders o ON ol.OrderId = o.OrderId
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
JOIN Orders o ON ol.OrderId = o.OrderId
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

        public async Task<List<(string, long)>> GetMostSalesMerchandises(string classification, DateTime timeBefore)
        {
            var sum = classification switch
            {
                "quantity" => "ol.Quantity",
                "profit" => "ol.Quantity * ol.Price * ol.Discount / 100",
                _ => string.Empty
            };
            if (string.IsNullOrEmpty(sum) || timeBefore > DateTime.Now) return new();

            string sql = $@"
SELECT TOP 5   m.MerchandiseName + s.specName  , SUM({sum}) as Data
FROM Merchandises m 
JOIN Specs s ON m.MerchandiseID = s.MerchandiseID
JOIN OrderLists ol ON s.SpecId = ol.SpecId
JOIN Orders o ON ol.OrderId = o.OrderId
WHERE o.PurchaseTime > @TimeBefore
GROUP BY MerchandiseName , specName , s.specId
ORDER BY SUM({sum}) DESC";

            var conn = _context.Database.GetDbConnection();
            return (await conn.QueryAsync<(string, long)>(sql, new { TimeBefore = timeBefore })).ToList();
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
JOIN Orders o on ol.OrderId = o.OrderId
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
JOIN Orders o ON ol.OrderId = o.OrderId
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
JOIN Orders o ON ol.OrderId = o.OrderId
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
            var result = await conn.QueryAsync<(string Label, long Data)>(sql, new { StartTime = startDate, EndTime = endDate });
            return result;
        }


        public async Task<int[]?> GetEvaluationScores(int merchandiseId)
        {
            string sql = @"SELECT
SUM(CASE WHEN Score = 5 THEN 1 ELSE 0 END) AS Five,
SUM(CASE WHEN Score = 4 THEN 1 ELSE 0 END) AS Four,
SUM(CASE WHEN Score = 3 THEN 1 ELSE 0 END) AS Three,
SUM(CASE WHEN Score = 2 THEN 1 ELSE 0 END) AS Two,
SUM(CASE WHEN Score = 1 THEN 1 ELSE 0 END) AS One
FROM Evaluations
WHERE MerchandiseId = @MerchandiseId";

            var conn = _context.Database.GetDbConnection();
            (int Five, int Four, int Three, int Two, int One) = await conn
                .QuerySingleAsync<(int, int, int, int, int)>(sql, new { MerchandiseId = merchandiseId });

            return new int[] { Five, Four, Three, Two, One };
        }

        public async Task<string?> GetNameById(int id, string measurement)
        {
            var condition = measurement switch
            {
                "merchandise" => _context.Merchandises.Where(o => o.MerchandiseId == id).Select(o => o.MerchandiseName),
                "spec" => _context.Specs.Where(o => o.SpecId == id).Select(o => o.Merchandise.MerchandiseName + o.SpecName),
                _ => null
            };
            if (condition == null) return null;
            return await condition.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<(DateTime, DateTime, long)>?> GetMerchandiseTrend(
            string measurement,
            string classification,
            string timeUnit,
            int intervalNum,
            int id,
            int intervalTimes = 36)
        {
            var sum = classification switch
            {
                "quantity" => "ol.Quantity",
                "profit" => "ol.Quantity * ol.Price * ol.Discount / 100",
                _ => string.Empty
            };

            string condition = measurement switch
            {
                "merchandise" => "m.MerchandiseId",
                "spec" => "s.specId",
                _ => string.Empty
            };

            string sqlTimeUnit = timeUnit switch
            {
                "day" => "DAY",
                "month" => "MONTH",
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(sum) || string.IsNullOrEmpty(condition) || string.IsNullOrEmpty(sqlTimeUnit)) return null;

            string sql = $@"
WITH DateRangeCTE AS (
    SELECT 
        DATEADD({sqlTimeUnit}, @IntervalNum , @DateNow) AS StartDate,
        DATEADD(MILLISECOND, -2, @DateNow) AS EndDate,
		1 AS CountNum
    UNION ALL
    SELECT
        DATEADD({sqlTimeUnit}, @IntervalNum, StartDate),
        DATEADD({sqlTimeUnit}, @IntervalNum, EndDate),
		CountNum + 1
    FROM DateRangeCTE
    WHERE CountNum < @IntervalTimes
),
Result AS (
SELECT DateRangeCTE.StartDate ,DateRangeCTE.EndDate , SUM({sum}) AS Data 
FROM Merchandises m 
JOIN Specs s on m.MerchandiseID = s.MerchandiseId
JOIN OrderLists ol on s.SpecId = ol.SpecId
JOIN Orders o on ol.OrderId = o.OrderId
RIGHT JOIN  DateRangeCTE on DateRangeCTE.StartDate < o.PurchaseTime AND DateRangeCTE.EndDate > o.PurchaseTime
WHERE {condition} = @Id
GROUP BY DateRangeCTE.StartDate , DateRangeCTE.EndDate
)
SELECT DateRangeCTE.StartDate , DateRangeCTE.EndDate , Result.Data
FROM Result
RIGHT JOIN DateRangeCTE ON Result.StartDate = DateRangeCTE.StartDate
ORDER BY DateRangeCTE.StartDate";

            using var conn = _context.Database.GetDbConnection();

            return await conn.QueryAsync<(DateTime, DateTime, long)>(sql, new
            {
                DateNow = DateTime.Now.Date,
                IntervalNum = -intervalNum,
                Id = id,
                IntervalTimes = intervalTimes
            });
        }






        public async Task<List<string>?> GetAutoCompleteNames(string queryCol, string keyword)
        {
            queryCol = queryCol.Trim().ToLower();
            var queryName = queryCol switch
            {
                "merchandisename" => "MerchandiseName",
                "merchandiseid" => "CAST( Merchandises.MerchandiseId AS NVARCHAR)",
                "specname" => "MerchandiseName + SpecName",
                "specid" => "CAST( SpecId AS NVARCHAR)",
                _ => string.Empty,
            };

            var conditionCol = queryCol switch
            {
                "merchandisename" => "MerchandiseName",
                "merchandiseid" => "Merchandises.MerchandiseId ",
                "specname" => "MerchandiseName + SpecName",
                "specid" => " SpecId ",
                _ => string.Empty,
            };
            if (string.IsNullOrEmpty(keyword)
                || string.IsNullOrEmpty(conditionCol)
                || string.IsNullOrEmpty(queryName)) { return new(); }

            string sql = $@"  
SELECT DISTINCT TOP 20 {queryName}
FROM Merchandises
JOIN Specs ON Merchandises.MerchandiseId = Specs.MerchandiseId
WHERE CAST( {conditionCol} AS NVARCHAR) LIKE '%' + @Keyword + '%' ";

            using var conn = _context.Database.GetDbConnection();
            return (await conn.QueryAsync<string>(sql, new { Keyword = keyword })).ToList();
        }

        public async Task<(int, string)> GetSearchedId(string queryCol, string keyword)
        {
            queryCol = queryCol.Trim().ToLower();
            var queryName = queryCol switch
            {
                "merchandisename" => "Merchandises.MerchandiseId, MerchandiseName ",
                "merchandiseid" => "Merchandises.MerchandiseId , MerchandiseName ",
                "specname" => "SpecId , MerchandiseName + SpecName ",
                "specid" => "SpecId , MerchandiseName + SpecName ",
                _ => string.Empty,
            };

            var conditionCol = queryCol switch
            {
                "merchandisename" => "MerchandiseName",
                "merchandiseid" => "Merchandises.MerchandiseId ",
                "specname" => "MerchandiseName + SpecName",
                "specid" => " SpecId ",
                _ => string.Empty,
            };
            if (string.IsNullOrEmpty(keyword)
                || string.IsNullOrEmpty(conditionCol)
                || string.IsNullOrEmpty(queryName)) { return new(); }

            string sql = $@"  
SELECT DISTINCT TOP 1 {queryName}
FROM Merchandises
JOIN Specs ON Merchandises.MerchandiseId = Specs.MerchandiseId
WHERE CAST( {conditionCol} AS NVARCHAR) =  @Keyword  ";

            using var conn = _context.Database.GetDbConnection();
            return await conn.QueryFirstAsync<(int, string)>(sql, new { Keyword = keyword });
        }




        public async Task<MerchandiseRadarDto?> GetMerchandiseRadar(string measurement, int id)
        {
            string condition = measurement switch
            {
                "merchandise" => "m.MerchandiseId",
                "spec" => "s.specId",
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(condition)) return null;

            string sql = $@"
WITH 
cte1(a,b) AS
(
SELECT 
{condition} 
,PERCENT_RANK() OVER (ORDER BY SUM(ol.quantity)) 
FROM Merchandises m 
JOIN Specs s on m.MerchandiseID = s.MerchandiseId
LEFT JOIN OrderLists ol on s.SpecId = ol.SpecId
Group by {condition} 
),
cte2(a,b) AS
(
SELECT 
{condition} 
,PERCENT_RANK() OVER (ORDER BY AVG(CAST(e.Score AS float))) 
FROM Merchandises m 
JOIN Specs s on m.MerchandiseID = s.MerchandiseId
LEFT JOIN Evaluations e on e.SpecId = s.SpecId
Group by {condition} 
),
cte3(a,b) AS
(
SELECT 
{condition} 
,PERCENT_RANK() OVER (ORDER BY sum(c.Quantity)) 
FROM Merchandises m 
JOIN Specs s on m.MerchandiseID = s.MerchandiseId
JOIN CartItems c on c.SpecId = s.SpecId
Group by {condition} 
),
cte４(a,b) AS
(
SELECT 
{condition} 
,PERCENT_RANK() OVER (ORDER BY  SUM(ol.quantity) )
FROM Merchandises m 
JOIN Specs s on m.MerchandiseID = s.MerchandiseId
LEFT JOIN OrderLists ol on s.SpecId = ol.SpecId
LEFT JOIN Orders o on ol.OrderId = o.OrderId
WHERE o.PurchaseTime > DATEADD ( DAY , -30 , GETDATE() )
GROUP BY {condition} 
),
cte5(a,b) AS
(
SELECT 
{condition} 
,PERCENT_RANK() OVER (ORDER BY AVG(CAST(e.Score AS float)))
FROM Merchandises m 
JOIN Specs s on m.MerchandiseID = s.MerchandiseId
JOIN OrderLists ol on s.SpecId = ol.SpecId
JOIN Orders o on ol.OrderId = o.OrderId
LEFT JOIN Evaluations e on e.SpecId = s.SpecId
WHERE o.PurchaseTime > DATEADD ( DAY , -30 , GETDATE() )
GROUP BY {condition} 
)
SELECT 
cte1.b AS BoughtRank
, cte4.b  AS RecentBoughtRank
, cte2.b AS EvaluationRank
, cte5.b  AS RecentEvaluationRank
, cte3.b AS InCartRank
FROM cte1
LEFT JOIN cte2 ON cte1.a = cte2.a
LEFT JOIN cte3 ON cte1.a = cte3.a
LEFT JOIN cte4 ON cte1.a = cte4.a
LEFT JOIN cte5 ON cte1.a = cte5.a
WHERE cte1.a = @Id
";
            using var conn = _context.Database.GetDbConnection();
            (double BoughtRank, double RecentBoughtRank, double EvaluationRank, double RecentEvaluationRank, double InCartRank)
                = await conn.QuerySingleAsync<(double, double, double, double, double)>(sql, new { Id = id });

            return new MerchandiseRadarDto
            {
                BoughtRank = Convert.ToInt32(BoughtRank * 100),
                RecentBoughtRank = Convert.ToInt32(RecentBoughtRank * 100),
                EvaluationRank = Convert.ToInt32(EvaluationRank * 100),
                RecentEvaluationRank = Convert.ToInt32(RecentEvaluationRank * 100),
                InCartRank = Convert.ToInt32(InCartRank * 100),
            };

        }





    }
}
