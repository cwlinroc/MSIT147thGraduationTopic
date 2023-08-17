using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Microsoft.Data.SqlClient;

namespace MSIT147thGraduationTopic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiHomeController : ControllerBase
    {

        private readonly GraduationTopicContext _context;
        private readonly HomeServices _service;
        private readonly IWebHostEnvironment _environment;
        public ApiHomeController(GraduationTopicContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _service = new HomeServices(context);
            _environment = environment;
        }


        //臨時資料庫輸入縣市地區資料
        #region 臨時

        //[HttpGet("insertcitiesanddistricts")]
        //public IActionResult InsertCitiesAndDistricts()
        //{
        //    return Content("dont do it");
        //    var fileProvider = new PhysicalFileProvider(_environment.WebRootPath);
        //    var fileInfo = fileProvider.GetFileInfo("datas/CityCountyData.json");
        //    using var stream = fileInfo.CreateReadStream();
        //    using var reader = new StreamReader(stream);

        //    JsonReader jsonReader = new JsonTextReader(reader);
        //    var json = (JArray)JToken.ReadFrom(jsonReader);

        //    var cities = json.Select(o => o["CityName"].ToString()).Select(o => new City { CityName = o });
        //    _context.Cities.AddRange(cities);
        //    _context.SaveChanges();
        //    //var cities = _context.Cities.ToList();

        //    foreach (var city in cities)
        //    {
        //        IEnumerable<(string name, string code)> districtData = json
        //            .FirstOrDefault(o => o["CityName"].ToString() == city.CityName)["AreaList"]
        //            .Select(o => (o["AreaName"].ToString(), o["ZipCode"].ToString()));
        //        var districts = districtData.Select(o =>
        //            new District { ZipCode = int.Parse(o.code), DistrictName = o.name, CityId = city.CityId });

        //        string str = "INSERT INTO Districts (ZipCode , CityId ,DistrictName )"
        //            + "values (@ZipCode ,@CityId ,@DistrictName)";
        //        foreach (var district in districts)
        //        {
        //            using var conn = new SqlConnection(_context.Database.GetConnectionString());
        //            conn.Execute(str, district);
        //        }
        //    }
        //    return Content("success");
        //}

        #endregion


    }
}
