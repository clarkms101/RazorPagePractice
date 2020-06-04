using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPageBlog.Data;
using RazorPageBlog.Model;

namespace RazorPageBlog.Pages.Admin
{
    public class EditModel : PageModel
    {
        private readonly RazorPageBlogDbContext _context;

        [BindProperty]
        public ArticleForPage ArticleForPage { get; set; }

        [BindProperty]
        public IFormFile CoverPhoto { get; set; }

        public List<SelectListItem> TagSelectItems { get; set; }

        public EditModel(RazorPageBlogDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.SingleOrDefaultAsync(m => m.Id == id);
            
            if (article == null)
            {
                return NotFound();
            }

            ArticleForPage = new ArticleForPage
            {
                Id = article.Id,
                Body = article.Body,
                Title = article.Title,
                CoverPhoto = article.CoverPhoto,
                CreateDate = article.CreateDate,
                TagList = article.Tags.Split(',').ToList()
            };

            // 下拉清單先選好之前有的Tag
            TagSelectItems = _context.TagCloud.ToList().Select(s => new SelectListItem()
            {
                Value = s.Name,
                Text = s.Name,
                Selected = ArticleForPage.TagList.Contains(s.Name)
            }).ToList();
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            var path = "";
            if (CoverPhoto != null && CoverPhoto.Length != 0)
            {
                //取得現在的靜態檔案路徑
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", CoverPhoto.FileName);
                await using var stream = new FileStream(path, FileMode.Create);
                await CoverPhoto.CopyToAsync(stream);
            }
            
            var article = await UpdateArticle(path);
            _context.Attach(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticlesExists(ArticleForPage.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToPage("./Index");
        }

        private async Task<Article> UpdateArticle(string path)
        {
            var article = await _context.Articles.SingleAsync(m => m.Id == ArticleForPage.Id);
            article.Body = ArticleForPage.Body;
            article.Tags = string.Join(",", ArticleForPage.TagList);
            article.Title = ArticleForPage.Title;
            if (string.IsNullOrWhiteSpace(path) == false)
            {
                article.CoverPhoto = path;
            }

            article.CreateDate = ArticleForPage.CreateDate;
            return article;
        }

        private bool ArticlesExists(Guid id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}