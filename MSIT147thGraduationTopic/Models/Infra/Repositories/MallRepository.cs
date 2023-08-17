using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class MallRepository
    {
        private readonly GraduationTopicContext _context;
        public MallRepository(GraduationTopicContext context)
        {
            _context = context;
        }

        public IEnumerable<MallDisplay> getBasicMallDisplay(string txtKeyword, int searchCondition, int? minPrice, int? maxPrice)
        {
            IEnumerable<MallDisplay> datas = _context.MallDisplays
                .Where(md => md.Display == true).Where(md => md.OnShelf == true).Where(md => md.Amount > 0);

            if (!string.IsNullOrEmpty(txtKeyword))
            {
                datas = searchCondition switch
                {
                    1 => datas.Where(md => md.FullName.Contains(txtKeyword)),
                    2 => datas.Where(md => md.BrandName.Contains(txtKeyword)),
                    3 => datas.Where(md => md.CategoryName.Contains(txtKeyword)),
                    _ => datas
                };
            }
            datas = (minPrice.HasValue) ? datas.Where(sp => Math.Floor((double)sp.Price * sp.DiscountPercentage / 100) >= minPrice) : datas;
            datas = (maxPrice.HasValue) ? datas.Where(sp => Math.Floor((double)sp.Price * sp.DiscountPercentage / 100) <= maxPrice) : datas;

            return datas;
        }
    }
}
