using MSIT147thGraduationTopic.EFModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class SpecWithMerchandiseNameVM
    {
        private SpecWithMerchandiseName _specWm;
        public SpecWithMerchandiseName spec
        {
            get { return _specWm; }
            set { _specWm = value; }
        }
        public SpecWithMerchandiseNameVM()
        {
            _specWm = new SpecWithMerchandiseName();
        }

        [DisplayName("規格ID")]
        public int SpecId
        {
            get { return _specWm.SpecId; }
            set { _specWm.SpecId = value; }
        }
        [DisplayName("規格名稱")]
        public string SpecName
        {
            get { return _specWm.SpecName; }
            set { _specWm.SpecName = value; }
        }
        [DisplayName("商品ID")]
        public int MerchandiseId
        {
            get { return _specWm.MerchandiseId; }
            set { _specWm.MerchandiseId = value; }
        }
        [DisplayName("規格名稱")]
        public string MerchandiseName
        {
            get { return _specWm.MerchandiseName; }
            set { _specWm.MerchandiseName = value; }
        }

        [DisplayName("價格")]
        [Required(ErrorMessage = "此為必填欄位")]
        [Range(1, int.MaxValue, ErrorMessage = "價格須為大於0的數字")]
        public int Price
        {
            get { return _specWm.Price; }
            set { _specWm.Price = value; }
        }

        [DisplayName("庫存數量")]
        [Required(ErrorMessage = "此為必填欄位")]
        [Range(0, int.MaxValue, ErrorMessage = "庫存數量應至少為0")]
        public int Amount
        {
            get { return _specWm.Amount; }
            set { _specWm.Amount = value; }
        }

        [DisplayName("折扣比例")]
        public int DiscountPercentage
        {
            get { return _specWm.DiscountPercentage; }
            set { _specWm.DiscountPercentage = value; }
        }
        [DisplayName("顯示順序")]
        public int DisplayOrder
        {
            get { return _specWm.DisplayOrder; }
            set { _specWm.DisplayOrder = value; }
        }
        [DisplayName("上架此規格")]
        public bool OnShelf
        {
            get { return _specWm.OnShelf; }
            set { _specWm.OnShelf = value; }
        }
    }
}
