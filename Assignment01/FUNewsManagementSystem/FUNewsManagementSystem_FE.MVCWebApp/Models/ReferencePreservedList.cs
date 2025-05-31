namespace FUNewsManagementSystem_FE.MVCWebApp.Models
{
    public class ReferencePreservedList<T> where T : class
    {
        public string Id { get; set; } // Matches "$id"
        public List<T> Values { get; set; } // Matches "$values"
    }
}
