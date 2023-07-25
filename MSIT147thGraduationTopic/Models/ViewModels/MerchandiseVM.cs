using MSIT147thGraduationTopic.EFModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class MerchandiseVM
    {
        private Merchandise _merchandise = null;
        public Merchandise merchandise
        {
            get { return _merchandise; }
            set { _merchandise = value; }
        }
        public MerchandiseVM()
        {
            _merchandise = new Merchandise();
        }

        [DisplayName("商品ID")]
        public int MerchandiseId
        {
            get { return _merchandise.MerchandiseId; }
            set { _merchandise.MerchandiseId = value; }
        }
        [DisplayName("商品名稱")]
        [Required(ErrorMessage = "此為必填欄位")]
        [StringLength(30, ErrorMessage = "{0}不可超過{1}字")]
        public string MerchandiseName
        {
            get { return _merchandise.MerchandiseName; }
            set { _merchandise.MerchandiseName = value; }
        }
        [DisplayName("商品品牌")]
        [Required(ErrorMessage = "此為必選欄位")]
        public int BrandId
        {
            get { return _merchandise.BrandId; }
            set { _merchandise.BrandId = value; }
        }
        [DisplayName("商品類別")]
        [Required(ErrorMessage = "此為必選欄位")]
        public int CategoryId
{
            get { return _merchandise.CategoryId; }
            set { _merchandise.CategoryId = value; }
        }
        [DisplayName("商品描述")]
        [MaxLength(500, ErrorMessage = "{0}長度不可超過{1}字")]
        public string Description
        {
            get { return _merchandise.Description; }
            set { _merchandise.Description = value; }
        }
        [DisplayName("商品圖片")]
        public string ImageUrl
        {
            get { return _merchandise.ImageUrl; }
            set { _merchandise.ImageUrl = value; }
        }
        [DisplayName("展示商品")]
        public bool Display
        {
            get { return _merchandise.Display; }
            set { _merchandise.Display = value; }
        }
        public IFormFile photo { get; set; }
    }
}
