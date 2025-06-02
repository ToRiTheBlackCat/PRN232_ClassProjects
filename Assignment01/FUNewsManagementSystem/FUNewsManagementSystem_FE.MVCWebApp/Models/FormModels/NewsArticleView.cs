﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem_FE.MVCWebApp.Models.FormModels
{
    public class NewsArticleView
    {
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

        public List<TagView> TagsList { get; set; } = new();
    }
}
