namespace FUNewsManagementSystem_FE.MVCWebApp.Models
{
    public class CategoryModel
    {
        public short CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryDesciption { get; set; }

        public bool? IsActive { get; set; }
    }
}
