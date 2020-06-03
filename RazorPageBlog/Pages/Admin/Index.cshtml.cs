using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageBlog.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace RazorPageBlog.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly RazorPageBlogDbContext _context;
        private readonly int _pageSize;

        public IndexModel(RazorPageBlogDbContext context)
        {
            _context = context;
            _pageSize = 10;
        }

        public IEnumerable<Article> Articles { get; set; }

        public async Task OnGetAsync(string q, int? p)
        {
            var pageIndex = PageIndex(p);
            var query= _context.Articles.AsQueryable();
            if (string.IsNullOrWhiteSpace(q)==false)
            {
                query = query.Where(d => d.Title.Contains(q));
            }
            Articles = await query.ToPagedListAsync(pageIndex, _pageSize);
        }

        private static int PageIndex(int? p)
        {
            var pageIndex = p.HasValue ? p.Value < 1 ? 1 : p.Value : 1;
            return pageIndex;
        }
    }
}