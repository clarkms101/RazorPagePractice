using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorPageBlog.Helper.Tag
{
    public class TagsHelper : TagHelper
    {
        private IUrlHelperFactory UrlHelperFactory { get; }
        private IActionContextAccessor Accessor { get; }

        public TagsHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor accessor)
        {
            UrlHelperFactory = urlHelperFactory;
            Accessor = accessor;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var actionContext = Accessor.ActionContext;
            var urlHelper = UrlHelperFactory.GetUrlHelper(actionContext);
            output.TagName = "ul";
            var content = await output.GetChildContentAsync();
            if (string.IsNullOrWhiteSpace(content.GetContent()) == false)
            {
                var tags = content.GetContent().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                var sb = new StringBuilder();
                foreach (var tag in tags)
                {
                    sb.AppendFormat("<li class='d-inline p-1'><a href='{0}' class='btn btn-success'>{1}</a></li>",
                        urlHelper.Page("index", "tag", new {tag = tag}), tag);
                }

                output.Content.SetHtmlContent(sb.ToString());
            }
        }
    }
}