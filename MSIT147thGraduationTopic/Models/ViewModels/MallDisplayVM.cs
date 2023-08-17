using MSIT147thGraduationTopic.EFModels;
using System.ComponentModel;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class MallDisplayVM
    {
        private MallDisplay _malldisplay;
        public MallDisplay malldisplay
        {
            get { return _malldisplay; }
            set { _malldisplay = value; }
        }
        public int SpecId
        {
            get { return _malldisplay.SpecId; }
            set { _malldisplay.SpecId = value; }
        }
        public int MerchandiseId 
        {
            get { return _malldisplay.MerchandiseId; }
            set { _malldisplay.MerchandiseId = value; }
        }
        public string BrandName
        {
            get { return _malldisplay.BrandName; }
            set { _malldisplay.BrandName = value; }
        }
        public string CategoryName
        {
            get { return _malldisplay.CategoryName; }
            set { _malldisplay.CategoryName = value; }
        }
        public int CategoryId
        {
            get { return _malldisplay.CategoryId; }
            set { _malldisplay.CategoryId = value; }
        }
        public string MerchandiseName
        {
            get { return _malldisplay.MerchandiseName; }
            set { _malldisplay.MerchandiseName = value; }
        }
        public string SpecName
        {
            get { return _malldisplay.SpecName; }
            set { _malldisplay.SpecName = value; }
        }
        public string FullName
        {
            get { return _malldisplay.FullName; }
            set { _malldisplay.FullName = value; }
        }
        public int Price
        {
            get { return _malldisplay.Price; }
            set { _malldisplay.Price = value; }
        }
        public int Amount
        {
            get { return _malldisplay.Amount; }
            set { _malldisplay.Amount = value; }
        }
        public int DiscountPercentage
        {
            get { return _malldisplay.DiscountPercentage; }
            set { _malldisplay.DiscountPercentage = value; }
        }
        public string MerchandiseImageUrl
        {
            get { return _malldisplay.MerchandiseImageUrl; }
            set { _malldisplay.MerchandiseImageUrl = value; }
        }
        public string SpecImageUrl
        {
            get { return _malldisplay.SpecImageUrl; }
            set { _malldisplay.SpecImageUrl = value; }
        }
        public double Popularity
        {
            get { return _malldisplay.Popularity; }
            set { _malldisplay.Popularity = value; }
        }
        public bool Display
        {
            get { return _malldisplay.Display; }
            set { _malldisplay.Display = value; }
        }
        public bool OnShelf
        {
            get { return _malldisplay.OnShelf; }
            set { _malldisplay.OnShelf = value; }
        }
        public double Score { get; set; }
    }
}
