using System.ComponentModel.DataAnnotations;

namespace ProductManagementMVC.Models
{
    public class LoginRequest
    {
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string MemberPassword { get; set; }

    }
}
