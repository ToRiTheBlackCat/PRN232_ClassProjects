using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem_BE.API.Models
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
