using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Models.Dtos.Recommend
{
    public class ManuallyWeightedEntryDto
    {
        public int EntryId { get; set; }
        public int? TagId { get; set; }
        public int? MerchandiseId { get; set; }
        public int? SpecId { get; set; }
        public int Weight { get; set; }
    }


    static public class ManuallyWeightedTransfer
    {
        static public ManuallyWeightedEntryDto ToDto(this ManuallyWeightedEntry entity)
        {
            return new()
            {
                EntryId = entity.EntryId,
                SpecId = entity.SpecId,
                MerchandiseId = entity.MerchandiseId,
                TagId = entity.TagId,
                Weight = entity.Weight
            };
        }
    }



}

