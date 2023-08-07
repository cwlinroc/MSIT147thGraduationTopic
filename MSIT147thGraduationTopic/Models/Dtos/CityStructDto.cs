using Microsoft.CodeAnalysis.CSharp.Syntax;
using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Models.Dtos
{
    public class CityStructDto
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public List<DistrictStructDto>? Districts { get; set; }
    }

    public class DistrictStructDto
    {
        public int DistrictId { get; set; }
        public int ZipCode { get; set; }
        public int CityId { get; set; }
        public string DistrictName { get; set; }
    }

    static public class CityStructTransfer
    {
        static public CityStructDto ToDto(this City entity)
        {
            return new CityStructDto
            {
                CityId = entity.CityId,
                CityName = entity.CityName
            };
        }

        static public DistrictStructDto ToDto(this District entity)
        {
            return new DistrictStructDto
            {
                DistrictId = entity.DistrictId,
                ZipCode = entity.ZipCode,
                CityId = entity.CityId,
                DistrictName = entity.DistrictName
            };
        }
    }



}
