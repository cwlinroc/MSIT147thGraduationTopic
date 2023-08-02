using MSIT147thGraduationTopic.EFModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class MerchandiseVM
    {
        private Merchandise _merchandise;
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
        public string MerchandiseName
        {
            get { return _merchandise.MerchandiseName; }
            set { _merchandise.MerchandiseName = value; }
        }
        [DisplayName("商品品牌")]
        public int BrandId
        {
            get { return _merchandise.BrandId; }
            set { _merchandise.BrandId = value; }
        }
        [DisplayName("商品類別")]
        public int CategoryId
{
            get { return _merchandise.CategoryId; }
            set { _merchandise.CategoryId = value; }
        }
        [DisplayName("商品描述")]
        public string? Description
        {
            get { return _merchandise.Description; }
            set { _merchandise.Description = value; }
        }
        [DisplayName("商品圖片")]
        public string? ImageUrl
        {
            get { return _merchandise.ImageUrl; }
            set { _merchandise.ImageUrl = value; }
        }
        [DisplayName("於商城展示商品")]
        public bool Display
        {
            get { return _merchandise.Display; }
            set { _merchandise.Display = value; }
        }
        public IFormFile? photo { get; set; }
        public bool? deleteImageIndicater { get; set; }
    }
}
