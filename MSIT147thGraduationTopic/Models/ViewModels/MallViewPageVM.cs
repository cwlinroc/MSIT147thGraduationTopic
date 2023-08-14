using MSIT147thGraduationTopic.EFModels;
using System.ComponentModel;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class MallViewPageVM
    {
        private Spec _spec;
        public Spec? spec
        {
            get { return _spec; }
            set { _spec = value; }
        }
        public MallViewPageVM()
        {
            _spec = new Spec();
        }

        [DisplayName("規格ID")]
        public int SpecId
        {
            get { return _spec.SpecId; }
            set { _spec.SpecId = value; }
        }
        [DisplayName("規格名稱")]
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
        public int Price
        {
            get { return _spec.Price; }
            set { _spec.Price = value; }
        }

        [DisplayName("庫存數量")]
        public int Amount
        {
            get { return _spec.Amount; }
            set { _spec.Amount = value; }
        }

        [DisplayName("規格圖片")]
        public string? ImageUrl
        {
            get { return _spec.ImageUrl; }
            set { _spec.ImageUrl = value; }
        }

        [DisplayName("折扣比例(%)")]
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

        [DisplayName("熱門度")]
        public double Popularity
        {
            get { return _spec.Popularity; }
            set { _spec.Popularity = value; }
        }

        [DisplayName("上架此規格")]
        public bool OnShelf
        {
            get { return _spec.OnShelf; }
            set { _spec.OnShelf = value; }
        }
        public double Score { get; set; }
    }
}
