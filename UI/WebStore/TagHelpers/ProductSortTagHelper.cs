using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebStore.Domain.Models;

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

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new (); 

        public ProductSortTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "a";

            PageUrlValues["SortOrder"] = Property;
            var url = urlHelper.Action(Action, PageUrlValues);

            output.Attributes.SetAttribute("href", url);

            if (Current == Property)
            {
                var tag = new TagBuilder("i");
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
