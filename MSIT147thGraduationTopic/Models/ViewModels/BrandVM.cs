using MSIT147thGraduationTopic.EFModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class BrandVM
    {
        private Brand _brand;
        public Brand brand
        {
            get { return _brand; }
            set { _brand = value; }
        }
        public BrandVM()
        {
            _brand = new Brand();
        }

        [DisplayName("品牌ID")]
        public int BrandId
        {
            get { return _brand.BrandId; }
            set { _brand.BrandId = value; }
        }
        [DisplayName("品牌名稱")]
        [Required(ErrorMessage = "此為必填欄位")]
        public string BrandName
        {
            get { return _brand.BrandName; }
            set { _brand.BrandName = value; }
        }
    }
}
