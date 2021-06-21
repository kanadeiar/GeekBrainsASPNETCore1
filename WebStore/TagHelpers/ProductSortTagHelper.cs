using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebStore.Models;

namespace WebStore.TagHelpers
{
    /// <summary> Таг хелпер сортировки продуктов </summary>
    public class ProductSortTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public ProductSortState Property { get; set; }
        public ProductSortState Current { get; set; }
        public string Action { get; set; }
        public bool Up { get; set; }
        
        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public ProductSortTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "a";
            var url = urlHelper.Action(Action, new {SortOrder = Property});
            output.Attributes.SetAttribute("href", url);

            if (Current == Property)
            {
                TagBuilder tag = new TagBuilder("i");
                tag.AddCssClass("glyphicon");
                if (Up == true)
                    tag.AddCssClass("glyphicon-chevron-up");
                else
                    tag.AddCssClass("glyphicon-chevron-down");

                output.PreContent.AppendHtml(tag);
            }
        }
    }
}
