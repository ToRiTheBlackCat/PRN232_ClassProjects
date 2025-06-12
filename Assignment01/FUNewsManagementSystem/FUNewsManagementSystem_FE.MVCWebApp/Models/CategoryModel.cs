using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem_FE.MVCWebApp.Models
{
    public class CategoryModel
    {
        public short CategoryId { get; set; }
        [Required] 
        public string CategoryName { get; set; }
        [Required]
        public string CategoryDesciption { get; set; }
        public short? ParentCategoryId { get; set; }
        public bool? IsActive { get; set; }
    }
}
