using MSIT147thGraduationTopic.EFModels;
using System.ComponentModel;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class TagVM
    {
        private Tag _tag;
        public Tag? tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        public TagVM()
        {
            _tag = new Tag();
        }

        [DisplayName("標籤ID")]
        public int TagId
        {
            get { return _tag.TagId; }
            set { _tag.TagId = value; }
        }

        [DisplayName("標籤名稱")]
        public string TagName
        {
            get { return _tag.TagName; }
            set { _tag.TagName = value; }
        }

        public int? matchedMerchandiseNumber { get; set; }
    }
}
