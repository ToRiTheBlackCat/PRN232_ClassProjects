using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUNewsManagementSystem_FE.RazorPageWebApp.Models
{
    public class NewsArticleModel
    {
        [Key]
        public string NewsArticleId { get; set; }

        public string NewsTitle { get; set; }

        public string Headline { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string NewsContent { get; set; }

        public string NewsSource { get; set; }

        public short? CategoryId { get; set; } = 1;

        public bool? NewsStatus { get; set; }

        public short? CreatedById { get; set; }

        public short? UpdatedById { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public CategoryModel Category { get; set; }
        [JsonPropertyName("tags")]
        public List<TagModel> Tags { get; set; }
    }
}
