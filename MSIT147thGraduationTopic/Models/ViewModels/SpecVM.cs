using MSIT147thGraduationTopic.EFModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class SpecVM
    {
        private Spec _spec;
        public Spec spec
        {
            get { return _spec; }
            set { _spec = value; }
        }
        public SpecVM()
        {
            _spec = new Spec();
        }

        [DisplayName("規格ID")]
        public int SpecId
        {
            get { return _spec.SpecId; }
            set { _spec.SpecId = value; }
        }
        [DisplayName("品牌名稱")]
        [Required(ErrorMessage = "此為必填欄位")]
        public string SpecName
        {
            get { return _spec.SpecName; }
            set { _spec.SpecName = value; }
        }
        [DisplayName("商品ID")]
        public int MerchandiseId
        {
            get { return _spec.MerchandiseId; }
            set { _spec.MerchandiseId = value; }
        }

        [DisplayName("價格")]
        [Required(ErrorMessage = "此為必填欄位")]
        [Range(1, int.MaxValue, ErrorMessage = "價格須為大於0的數字")]
        public int Price
        {
            get { return _spec.Price; }
            set { _spec.Price = value; }
        }

        [DisplayName("庫存數量")]
        [Required(ErrorMessage = "此為必填欄位")]
        [Range(0, int.MaxValue, ErrorMessage = "庫存數量應至少為0")]
        public int Amount
        {
            get { return _spec.Amount; }
            set { _spec.Amount = value; }
        }
        [DisplayName("折扣比例")]
        public int DiscountPercentage
        {
            get { return _spec.DiscountPercentage; }
            set { _spec.DiscountPercentage = value; }
        }
        [DisplayName("顯示順序")]
        public int DisplayOrder
        {
            get { return _spec.DisplayOrder; }
            set { _spec.DisplayOrder = value; }
        }
        [DisplayName("上架此規格")]
        public bool OnShelf
        {
            get { return _spec.OnShelf; }
            set { _spec.OnShelf = value; }
        }
    }
}
