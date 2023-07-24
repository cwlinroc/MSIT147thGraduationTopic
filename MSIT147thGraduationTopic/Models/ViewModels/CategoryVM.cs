﻿using MSIT147thGraduationTopic.EFModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class CategoryVM
    {

        private Category _category = null;
        public Category category
        {
            get { return _category; }
            set { _category = value; }
        }
        public CategoryVM()
        {
            _category = new Category();
        }

        [DisplayName("類別ID")]
        public int CategoryId
        {
            get { return _category.CategoryId; }
            set { _category.CategoryId = value; }
        }
        [DisplayName("類別名稱")]
        [Required(ErrorMessage = "此為必填欄位")]
        public string CategoryName
        {
            get { return _category.CategoryName; }
            set { _category.CategoryName = value; }
        }
    }
}
