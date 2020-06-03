using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageBlog.Data;
using RazorPageBlog.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace RazorPageBlog.Pages.Admin
{
    public class CreateModel : PageModel
    {
        private readonly RazorPageBlogDbContext _context;

        public CreateModel(RazorPageBlogDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ArticleForPage ArticleForPage { get; set; }

        [BindProperty]
        [Required]
        public IFormFile CoverPhoto { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }
            
            if (CoverPhoto == null || CoverPhoto.Length == 0)
                return Content("coverPhoto not selected");
            //取得現在的靜態檔案路徑
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", CoverPhoto.FileName);

            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await CoverPhoto.CopyToAsync(stream);
            }

            var article = new Article
            {
                Id = Guid.NewGuid(),
                Body = ArticleForPage.Body,
                Tags = ArticleForPage.Tags,
                Title = ArticleForPage.Title,
                CoverPhoto = path,
                CreateDate = ArticleForPage.CreateDate
            };
            
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}