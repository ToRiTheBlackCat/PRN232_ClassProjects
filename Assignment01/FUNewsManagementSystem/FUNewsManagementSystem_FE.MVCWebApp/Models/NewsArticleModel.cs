﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUNewsManagementSystem_FE.MVCWebApp.Models
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

        public short? CategoryId { get; set; }

        public bool? NewsStatus { get; set; }

        public short? CreatedById { get; set; }

        public short? UpdatedById { get; set; }

        public DateTime? ModifiedDate { get; set; }
        [JsonPropertyName("tags")]
        public ReferencePreservedList<TagModel> Tags { get; set; }
    }
}
