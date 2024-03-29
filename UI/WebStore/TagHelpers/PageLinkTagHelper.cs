﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebStore.Domain.WebModels.Shared;

namespace WebStore.TagHelpers
{
    /// <summary> Таг хелпер пагинации </summary>
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public PageWebModel PageModel { get; set; }
        public string PageAction { get; set; }

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new (); 

        public PageLinkTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "div";

            var tag = new TagBuilder("ul");
            tag.AddCssClass("pagination");

            var currentItem = CreateTag(PageModel.Page, urlHelper);

            if (PageModel.HasPreviousPage)
            {
                var prevItem = CreatePrevious(PageModel.Page - 1, urlHelper);
                tag.InnerHtml.AppendHtml(prevItem);
            }
            if (PageModel.HasFirstPage)
            {
                var firstItem = CreateTag(1, urlHelper);
                tag.InnerHtml.AppendHtml(firstItem);
            }
            if (PageModel.HasPrevPreviousPage)
            {
                var prevPrevItem = CreateTag(PageModel.Page - 2, urlHelper);
                tag.InnerHtml.AppendHtml(prevPrevItem);
            }
            if (PageModel.HasPreviousPage)
            {
                var prevItem = CreateTag(PageModel.Page - 1, urlHelper);
                tag.InnerHtml.AppendHtml(prevItem);
            }
            tag.InnerHtml.AppendHtml(currentItem);
            if (PageModel.HasNextPage)
            {
                var nextItem = CreateTag(PageModel.Page + 1, urlHelper);
                tag.InnerHtml.AppendHtml(nextItem);
            }
            if (PageModel.HasNextNextPage)
            {
                var nextNextItem = CreateTag(PageModel.Page + 2, urlHelper);
                tag.InnerHtml.AppendHtml(nextNextItem);
            }
            if (PageModel.HasLastPage)
            {
                var lastItem = CreateTag(PageModel.TotalPages, urlHelper);
                tag.InnerHtml.AppendHtml(lastItem);
            }
            if (PageModel.HasNextPage)
            {
                var nextItem = CreateNext(PageModel.Page + 1, urlHelper);
                tag.InnerHtml.AppendHtml(nextItem);
            }

            output.Content.AppendHtml(tag);
        }

        TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
        {
            var item = new TagBuilder("li");
            var link = new TagBuilder("a");
            if (pageNumber == PageModel.Page)
            {
                item.AddCssClass("active");
            }
            else
            {
                PageUrlValues["page"] = pageNumber;
                link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            }

            foreach (var (key, value) in PageUrlValues.Where(v => v.Value is not null))
                link.MergeAttribute($"data-{key}", value.ToString());

            item.AddCssClass("page-item");
            link.AddCssClass("page-link");
            link.InnerHtml.Append(pageNumber.ToString());
            item.InnerHtml.AppendHtml(link);
            return item;
        }

        TagBuilder CreatePrevious(int pageNumber, IUrlHelper urlHelper)
        {
            var item = new TagBuilder("li");
            var link = new TagBuilder("a");
            PageUrlValues["page"] = pageNumber;
            link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);

            foreach (var (key, value) in PageUrlValues.Where(v => v.Value is not null))
                link.MergeAttribute($"data-{key}", value.ToString());

            item.AddCssClass("page-item");
            link.AddCssClass("page-link");
            var icon = new TagBuilder("i");
            icon.AddCssClass("glyphicon");
            icon.AddCssClass("glyphicon-chevron-left");
            link.InnerHtml.AppendHtml(icon);
            link.InnerHtml.Append(" Назад");
            item.InnerHtml.AppendHtml(link);
            return item;
        }
        TagBuilder CreateNext(int pageNumber, IUrlHelper urlHelper)
        {
            var item = new TagBuilder("li");
            var link = new TagBuilder("a");
            PageUrlValues["page"] = pageNumber;
            link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);

            foreach (var (key, value) in PageUrlValues.Where(v => v.Value is not null))
                link.MergeAttribute($"data-{key}", value.ToString());

            item.AddCssClass("page-item");
            link.AddCssClass("page-link");
            link.InnerHtml.Append("Вперед ");
            var icon = new TagBuilder("i");
            icon.AddCssClass("glyphicon");
            icon.AddCssClass("glyphicon-chevron-right");
            link.InnerHtml.AppendHtml(icon);
            item.InnerHtml.AppendHtml(link);
            return item;
        }
    }
}
