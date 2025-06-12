using Newtonsoft.Json;

namespace FUNewsManagementSystem_FE.MVCWebApp.Models
{
    public class LoginRequest
    {
        [JsonProperty("accountEmail")]
        public string AccountEmail { get; set; }
        [JsonProperty("accountPassword")]
        public string AccountPassword { get; set; }
    }
}
