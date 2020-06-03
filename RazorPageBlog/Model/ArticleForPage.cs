using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPageBlog.Model
{
    public class ArticleForPage
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        public string CoverPhoto { get; set; }
        
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        
        public string Tags { get; set; }

        public List<string> TagList { get; set; }
    }
}