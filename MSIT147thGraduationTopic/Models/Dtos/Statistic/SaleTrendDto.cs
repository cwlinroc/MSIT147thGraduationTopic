namespace MSIT147thGraduationTopic.Models.Dtos.Statistic
{
    public class SaleTrendDto
    {
        public List<string> Labels { get; set; }
        public List<SaleTrendDataSetDto> DataSets { get; set; }
    }

    public class SaleTrendDataSetDto
    {
        public string Label { get; set; }
        public List<long> Data { get; set; }
    }
    


}
