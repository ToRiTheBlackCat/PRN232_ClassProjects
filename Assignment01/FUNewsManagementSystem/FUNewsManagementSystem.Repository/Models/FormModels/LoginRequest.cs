using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem_FE.MVCWebApp.Models
{
	public class LoginRequest
	{
		[Required]
		public string UserName { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
