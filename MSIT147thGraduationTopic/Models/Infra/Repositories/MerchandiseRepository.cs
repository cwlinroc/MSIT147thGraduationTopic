using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class MerchandiseRepository
    {
        private readonly GraduationTopicContext _context;
        public MerchandiseRepository(GraduationTopicContext context)
        {
            _context = context;
        }

        public IEnumerable<MerchandiseSearch> getBasicMerchandiseSearch(string txtKeyword, int searchCondition, int displaymode)
        {
            IEnumerable<MerchandiseSearch> datas = datas = _context.MerchandiseSearches;

            if (!string.IsNullOrEmpty(txtKeyword))
            {
                if (searchCondition == 1)
                    datas = datas.Where(ms => ms.MerchandiseName.Contains(txtKeyword));
                if (searchCondition == 2)
                {
                    IQueryable<int> merchandiseIdFormSpec = _context.Specs
                        .Where(s => s.SpecName.Contains(txtKeyword)).Select(s => s.MerchandiseId).Distinct();
                    #region 建立新集合承接符合項(占版面&耗資源，有更好的寫法↓)
                    //datas = null;

                    //List<MerchandiseSearch> templist = new List<MerchandiseSearch>();
                    //foreach (int id in merchandiseIdFormSpec)
                    //{
                    //    MerchandiseSearch unit = _context.MerchandiseSearches.Where(ms => ms.MerchandiseId == id).FirstOrDefault();
                    //    if (unit != null)
                    //        templist.Add(unit);
                    //}
                    //datas = templist;
                    #endregion
                    datas = datas.Where(ms => merchandiseIdFormSpec.Contains(ms.MerchandiseId));
                }
                if (searchCondition == 3)
                    datas = datas.Where(ms => ms.BrandName.Contains(txtKeyword));
                if (searchCondition == 4)
                    datas = datas.Where(ms => ms.CategoryName.Contains(txtKeyword));
            }
            datas = displaymode switch
            {
                0 => datas = datas.Where(s => s.Display == true),     //在商城顯示
                2 => datas = datas.Where(s => s.Display == false),    //在商城隱藏
                _ => datas = datas.Select(s => s)                     //全部商品
            };

            return datas;
        }
    }
}
