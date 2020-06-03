using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RazorPageBlog.Data;
using RazorPageBlog.Model;

namespace RazorPageBlog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RazorPageBlogDbContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly int _articleBodyLength;

        public IEnumerable<ArticleForPage> Articles { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, RazorPageBlogDbContext context)
        {
            _logger = logger;
            _context = context;
            _articleBodyLength = 200;
        }

        public void OnGet()
        {
            Articles = _context.Articles.Select(s => new ArticleForPage()
            {
                Id = s.Id,
                CoverPhoto = s.CoverPhoto,
                Title = s.Title,
                Body = s.Body.Substring(0, _articleBodyLength),
                CreateDate = s.CreateDate,
                Tags = s.Tags
            }).ToList();
        }
    }
}
