﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RazorPageBlog.Data;
using RazorPageBlog.Model;
using X.PagedList;

namespace RazorPageBlog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RazorPageBlogDbContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly int _articleBodyLength;
        private readonly int _pageSize;

        public IEnumerable<ArticleForPage> Articles { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, RazorPageBlogDbContext context)
        {
            _logger = logger;
            _context = context;
            _articleBodyLength = 200;
            _pageSize = 5;
        }

        public void OnGet(string q, int? p)
        {
            var pageIndex = PageIndex(p);
            var query = _context.Articles.AsQueryable();
            if (string.IsNullOrWhiteSpace(q) == false)
            {
                query = query.Where(d => d.Title.Contains(q));
            }

            Articles = query.Select(s => new ArticleForPage()
            {
                Id = s.Id,
                CoverPhoto = s.CoverPhoto,
                Title = s.Title,
                Body = s.Body.Substring(0, _articleBodyLength),
                CreateDate = s.CreateDate,
                Tags = s.Tags
            }).ToPagedList(pageIndex, _pageSize);
        }

        public void OnGetTag(string tag, int? p)
        {
            var pageIndex = PageIndex(p);

            var query = _context.Articles.AsQueryable();
            if (string.IsNullOrWhiteSpace(tag) == false)
            {
                query = query.Where(d => d.Tags.Contains(tag));
            }

            Articles = query
                .Select(d => new ArticleForPage
                {
                    Id = d.Id,
                    CoverPhoto = d.CoverPhoto,
                    Title = d.Title,
                    Body = d.Body.Substring(0, _articleBodyLength),
                    CreateDate = d.CreateDate,
                    Tags = d.Tags
                }).ToPagedList(pageIndex, _pageSize);
        }

        private static int PageIndex(int? p)
        {
            var pageIndex = p.HasValue ? p.Value < 1 ? 1 : p.Value : 1;
            return pageIndex;
        }
    }
}
