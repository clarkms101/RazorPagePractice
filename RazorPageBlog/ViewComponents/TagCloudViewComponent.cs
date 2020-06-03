using Microsoft.AspNetCore.Mvc;
using RazorPageBlog.Data;
using System.Linq;

namespace RazorPageBlog.ViewComponents
{
    public class TagCloudViewComponent : ViewComponent
    {
        private readonly RazorPageBlogDbContext _context;

        public TagCloudViewComponent(RazorPageBlogDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewData.Model = _context.TagCloud.ToDictionary(d => d.Name, d => d.Amount);
            return View();
        }
    }
}