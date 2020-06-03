using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPageBlog.Data;
using System;
using System.Threading.Tasks;

namespace RazorPageBlog.Pages.Admin
{
    public class DetailsModel : PageModel
    {
        private readonly RazorPageBlogDbContext _context;

        public DetailsModel(RazorPageBlogDbContext context)
        {
            _context = context;
        }

        public Article Article { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = await _context.Articles.SingleOrDefaultAsync(m => m.Id == id);

            if (Article == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}