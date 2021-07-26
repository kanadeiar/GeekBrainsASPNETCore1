using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebStore.TagHelpers
{
    [HtmlTargetElement(Attributes = attributeName)]
    public class MyActiveRouteTagHelper : TagHelper
    {
        private const string attributeName = "my-active-route";
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll(attributeName);
        }
    }
}
