using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.Dtos.Statistic
{
    public class SaleChartDto
    {
        public string[]? Labels { get; set; }
        public long[]? Data { get; set; }
        public string? MeasurementUnit { get; set; }
    }
}
