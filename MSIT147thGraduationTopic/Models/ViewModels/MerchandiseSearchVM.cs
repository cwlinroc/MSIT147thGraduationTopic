using MSIT147thGraduationTopic.EFModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class MerchandiseSearchVM
    {

        private MerchandiseSearch _merchandisesearch;
        public MerchandiseSearch merchandisesearch
        {
            get { return _merchandisesearch; }
            set { _merchandisesearch = value; }
        }
        public MerchandiseSearchVM()
        {
            _merchandisesearch = new MerchandiseSearch();
        }

        [DisplayName("商品ID")]
        public int MerchandiseId
        {
            get { return _merchandisesearch.MerchandiseId; }
            set { _merchandisesearch.MerchandiseId = value; }
        }
        [DisplayName("商品名稱")]
        [Required(ErrorMessage = "此為必填欄位")]
        [StringLength(30, ErrorMessage = "字數不得大於30字")]
        public string MerchandiseName
        {
            get { return _merchandisesearch.MerchandiseName; }
            set { _merchandisesearch.MerchandiseName = value; }
        }
        [DisplayName("品牌名稱")]
        [Required(ErrorMessage = "此為必選欄位")]
        public string BrandName
        {
            get { return _merchandisesearch.BrandName; }
            set { _merchandisesearch.BrandName = value; }
        }
        [DisplayName("類別名稱")]
        [Required(ErrorMessage = "此為必選欄位")]
        public string CategoryName
        {
            get { return _merchandisesearch.CategoryName; }
            set { _merchandisesearch.CategoryName = value; }
        }
        [DisplayName("商品描述")]
        [StringLength(50, ErrorMessage = "字數不得超過500字")]
        public string Description
        {
            get { return _merchandisesearch.Description; }
            set { _merchandisesearch.Description = value; }
        }
        [DisplayName("商品圖片")]
        public string ImageUrl
        {
            get { return _merchandisesearch.ImageUrl; }
            set { _merchandisesearch.ImageUrl = value; }
        }
        [DisplayName("於商城展示商品")]
        public bool Display
        {
            get { return _merchandisesearch.Display; }
            set { _merchandisesearch.Display = value; }
        }
    }
}
